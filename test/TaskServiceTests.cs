using FluentAssertions;
using TaskFlow.Models;
using TaskFlow.Services;
using Xunit;

namespace TaskFlow.Tests;

/// <summary>
/// Proporciona pruebas unitarias para la clase <see cref="TaskItemService"/>.
/// </summary>
public class TaskServiceTests
{
    private readonly TaskItemService _service;

    public TaskServiceTests()
    {
        _service = new TaskItemService();
    }

    /// <summary>
    /// Valida que una tarea se cree correctamente y se agregue a la lista.
    /// </summary>
    [Fact]
    public void CreateTask_ShouldAddTaskToList_WhenDataIsValid()
    {
        // Arrange
        var title = "Test Task";
        var description = "Description";
        var responsible = "QA";

        // Act
        _service.CreateTask(title, description, responsible);
        var tasks = _service.ListTasks();

        // Assert
        tasks.Should().HaveCount(1);
        var createdTask = tasks.FirstOrDefault(t => t.Title == title);
        createdTask.Should().NotBeNull();
        createdTask!.Title.Should().Be(title);
        createdTask.Description.Should().Be(description);
        createdTask.Responsible.Should().Be(responsible);
        createdTask.Status.Should().Be(TaskStatus.ToDo);
    }

    /// <summary>
    /// Valida que el estado de una tarea se actualice correctamente.
    /// </summary>
    [Fact]
    public void CreateTask_ValidDataWithoutDescription_ShouldAddTaskToList()
    {
        // Arrange
        var title = "Test Task";
        var responsible = "QA";

        // Act
        _service.CreateTask(title, responsible);
        var tasks = _service.ListTasks();

        // Assert
        tasks.Should().HaveCount(1);
        var createdTask = tasks.FirstOrDefault(t => t.Title == title);
        createdTask.Should().NotBeNull();
        createdTask!.Title.Should().Be(title);
        createdTask.Description.Should().BeNull();
        createdTask.Responsible.Should().Be(responsible);
        createdTask.Status.Should().Be(TaskStatus.ToDo);
    }

    /// <summary>
    /// Valida que el estado de una tarea se actualice correctamente.
    /// </summary>
    [Fact]
    public void UpdateTaskStatus_ShouldUpdateStatusAndUpdatedAt_WhenTaskExists()
    {
        // Arrange
        _service.CreateTask("Task 1", "Responsible 1");
        var task = _service.ListTasks()[0];
        var newStatus = TaskStatus.InProgress;

        // Act
        _service.UpdateTaskStatus(task.Id, newStatus);

        // Assert
        task.Status.Should().Be(newStatus);
        task.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Valida que se lance una excepción cuando se intenta actualizar una tarea inexistente.
    /// </summary>
    [Fact]
    public void UpdateTaskStatus_ShouldThrowArgumentException_WhenTaskDoesNotExist()
    {
        // Arrange
        var invalidId = 999;

        // Act
        var action = () => _service.UpdateTaskStatus(invalidId, TaskStatus.Done);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("Tarea no encontrada.");
    }

    /// <summary>
    /// Valida que se lance una excepción cuando el título está vacío.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateTask_ShouldThrowArgumentException_WhenTitleIsInvalid(string invalidTitle)
    {
        // Act
        var action = () => _service.CreateTask(invalidTitle, "Responsable");

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("El título de la tarea no puede estar vacío.");
    }

    /// <summary>
    /// Valida que se lance una excepción cuando el responsable está vacío.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateTask_ShouldThrowArgumentException_WhenResponsibleIsInvalid(string invalidResponsible)
    {
        // Act
        var action = () => _service.CreateTask("Título", invalidResponsible);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("El responsable de la tarea no puede estar vacío.");
    }
}
