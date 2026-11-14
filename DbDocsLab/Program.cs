using DbDocsLab.App.Data;
using DbDocsLab.App.Models;
using DbDocsLab.App.Repositories;

namespace DbDocsLab;

internal class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        try
        {
            using var context = new AppDbContext();

            var repository = new ProjectRepository(context);

            //создаём проект с версией документации.
            var project = new Project
            {
                Name = "Документация базы данных диплома",

                RepositoryUrl ="https://github.com/YOUR_LOGIN/dbdocs-lab",

                TrackedBranch = "main",

                DocumentationVersions =
                    new List<DocumentationVersion>
                    {
                        new DocumentationVersion
                        {
                            CommitSha = new string('a', 40),

                            DocumentationText ="Пример документации базы данных.",

                            CreatedAtUtc = DateTime.UtcNow
                        }
                    }
            };

            repository.Add(project);

            Console.WriteLine($"CREATE: создан проект с ID {project.Id}");

            //получаем список проектов.
            Console.WriteLine("READ: список проектов");

            foreach (var item in repository.GetAll())
            {
                Console.WriteLine($"{item.Id} | {item.Name} | {item.TrackedBranch}");
            }

            //находим проект по ID.
            var foundProject = repository.GetById(project.Id);

            if (foundProject is not null)
            {
                Console.WriteLine($"GET BY ID: {foundProject.Name}");

                Console.WriteLine($"Версий документации: " +$"{foundProject.DocumentationVersions.Count}");
            }

            //изменяем название проекта.
            bool updated = repository.UpdateName(
                project.Id,"Автоматизация документации БД");

            var updatedProject =repository.GetById(project.Id);

            Console.WriteLine($"UPDATE: {updated}");

            Console.WriteLine($"Новое название: {updatedProject?.Name}");

            // Создаём отдельный проект для проверки удаления.
            var temporaryProject = new Project
            {
                Name = "Временный проект",

                RepositoryUrl ="https://github.com/example/temporary",

                TrackedBranch = "main"
            };

            repository.Add(temporaryProject);

            int temporaryId = temporaryProject.Id;

            //удаляем временный проект.
            bool deleted = repository.Delete(temporaryId);

            Console.WriteLine($"DELETE: {deleted}");

            var deletedProject =repository.GetById(temporaryId);

            Console.WriteLine($"Проверка удаления: " +$"{(deletedProject is null ? "Не найден" : "Найден")}");

            Console.WriteLine("Проверка CRUD завершена.");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Ошибка: {exception.GetBaseException().Message}");

            Environment.ExitCode = 1;
        }
    }
}