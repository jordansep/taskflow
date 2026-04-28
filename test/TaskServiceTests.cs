using System;
using System.Text.Json;
using FluentAssertions;
using Moq;
using TaskFlow.Models;
using TaskFlow.Services;
using Xunit;

namespace TaskFlow.Tests;

/// <summary>
/// Proporciona pruebas unitarias para la clase <see cref="TaskItemService"/>.
/// </summary>
public class TaskServiceTests
{
    private readonly Mock<IFileManager> _fileManagerMock;
    private readonly TaskItemService _service;

    public TaskServiceTests()
    {
        _fileManagerMock = new Mock<IFileManager>();
        _service = new TaskItemService(_fileManagerMock.Object);
    }

    /// <summary>
    /// Valida que una tarea con todos los datos válidos se cree correctamente y se agregue a la lista.
    /// </summary>
    [Fact]
    public void CreateTask_ValidData_ShouldAddTaskToList()
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
    /// Valida que una tarea sin descripción se cree correctamente y se agregue a la lista.
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
    /// Valida que el estado de una tarea se actualice correctamente y se guarde en archivo usando la dependencia externa.
    /// </summary>
    [Fact]
    public void UpdateTaskStatus_ValidId_ShouldUpdateStatusAndCallFileManager()
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
        
        // Verifica que se haya llamado a la dependencia externa para persistir los cambios
        _fileManagerMock.Verify(f => f.WriteAllText(
            It.Is<string>(path => path == "tasks.json"), 
            It.IsAny<string>()), Times.Once);
    }

    /// <summary>
    /// Valida que se lance una excepción cuando se intenta actualizar una tarea inexistente.
    /// </summary>
    [Fact]
    public void UpdateTaskStatus_NonExistentId_ShouldThrowArgumentException()
    {
        // Arrange
        var invalidId = 999;

        // Act
        var action = () => _service.UpdateTaskStatus(invalidId, TaskStatus.Done);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("Tarea no encontrada.");
        
        // Verifica que NO se haya llamado a la dependencia externa ya que la operación falló
        _fileManagerMock.Verify(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    /// <summary>
    /// Valida que se lance una excepción cuando el título es nulo o vacío.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateTask_NullOrEmptyTitle_ShouldThrowArgumentException(string? invalidTitle)
    {
        // Act
        var action1 = () => _service.CreateTask(invalidTitle, "Description", "Responsable");
        var action2 = () => _service.CreateTask(invalidTitle, "Responsable");

        // Assert
        action1.Should().Throw<ArgumentException>().WithMessage("El título de la tarea no puede estar vacío.");
        action2.Should().Throw<ArgumentException>().WithMessage("El título de la tarea no puede estar vacío.");
    }

    /// <summary>
    /// Valida que se lance una excepción cuando el responsable es nulo o vacío.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateTask_NullOrEmptyResponsible_ShouldThrowArgumentException(string? invalidResponsible)
    {
        // Act
        var action1 = () => _service.CreateTask("Título", "Description", invalidResponsible);
        var action2 = () => _service.CreateTask("Título", invalidResponsible);

        // Assert
        action1.Should().Throw<ArgumentException>().WithMessage("El responsable de la tarea no puede estar vacío.");
        action2.Should().Throw<ArgumentException>().WithMessage("El responsable de la tarea no puede estar vacío.");
    }
}
