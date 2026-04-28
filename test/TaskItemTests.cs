using FluentAssertions;
using TaskFlow.Models;
using Xunit;

namespace TaskFlow.Tests;

/// <summary>
/// Proporciona pruebas unitarias para la clase <see cref="TaskItem"/>.
/// </summary>
public class TaskItemTests
{
    /// <summary>
    /// Valida que un objeto <see cref="TaskItem"/> se pueda crear correctamente con valores válidos.
    /// </summary>
    /// <returns>Nada (procedimiento de prueba).</returns>
    [Fact]
    public void TaskItem_ShouldInitializeCorrectly_WhenProvidedValidData()
    {
        // Arrange
        var id = 1;
        var title = "Implementar Tests";
        var description = "Crear suite de pruebas para TaskItem";
        var responsible = "QA Team";
        var status = TaskStatus.ToDo;
        var createdAt = DateTime.UtcNow;

        // Act
        var taskItem = new TaskItem
        {
            Id = id,
            Title = title,
            Description = description,
            Responsible = responsible,
            Status = status,
            CreatedAt = createdAt
        };

        // Assert
        taskItem.Id.Should().Be(id);
        taskItem.Title.Should().Be(title);
        taskItem.Description.Should().Be(description);
        taskItem.Responsible.Should().Be(responsible);
        taskItem.Status.Should().Be(status);
        taskItem.CreatedAt.Should().Be(createdAt);
    }

    /// <summary>
    /// Valida el comportamiento cuando se asigna un título vacío.
    /// </summary>
    /// <returns>Nada (procedimiento de prueba).</returns>
    [Fact]
    public void TaskItem_Title_CanBeEmpty()
    {
        // Arrange
        var taskItem = new TaskItem();

        // Act
        taskItem.Title = string.Empty;

        // Assert
        taskItem.Title.Should().BeEmpty();
    }

    /// <summary>
    /// Valida que el tiempo de creación por defecto sea reciente.
    /// </summary>
    /// <returns>Nada (procedimiento de prueba).</returns>
    [Fact]
    public void TaskItem_CreatedAt_ShouldHaveDefaultValueNearNow()
    {
        // Arrange & Act
        var taskItem = new TaskItem();

        // Assert
        taskItem.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Valida el manejo de IDs negativos.
    /// </summary>
    /// <returns>Nada (procedimiento de prueba).</returns>
    [Fact]
    public void TaskItem_Id_ShouldNotAllowNegativeValues()
    {
        // Arrange
        var taskItem = new TaskItem();
        var negativeId = -1;

        // Act
        Action action = () => taskItem.Id = negativeId;

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("El ID no puede ser negativo.");
    }

    /// <summary>
    /// Valida que UpdatedAt pueda ser nulo inicialmente.
    /// </summary>
    /// <returns>Nada (procedimiento de prueba).</returns>
    [Fact]
    public void TaskItem_UpdatedAt_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var taskItem = new TaskItem();

        // Assert
        taskItem.UpdatedAt.Should().BeNull();
    }
}
