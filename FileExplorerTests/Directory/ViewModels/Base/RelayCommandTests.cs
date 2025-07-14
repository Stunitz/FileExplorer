using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Input;
using FileExplorer;

namespace FileExplorer.Tests
{
    [TestClass]
    public class RelayCommandTests
    {
        [TestMethod]
        public void Constructor_SetsAction()
        {
            // Arrange
            bool executed = false;
            Action action = () => executed = true;

            // Act
            var command = new RelayCommand(action);
            command.Execute(null);

            // Assert
            Assert.IsTrue(executed, "Action should be executed by Execute method.");
        }

        [TestMethod]
        public void CanExecute_AlwaysReturnsTrue()
        {
            // Arrange
            var command = new RelayCommand(() => { });

            // Act & Assert
            Assert.IsTrue(command.CanExecute(null));
            Assert.IsTrue(command.CanExecute("any parameter"));
        }

        [TestMethod]
        public void Execute_InvokesAction()
        {
            // Arrange
            bool wasCalled = false;
            var command = new RelayCommand(() => wasCalled = true);

            // Act
            command.Execute(null);

            // Assert
            Assert.IsTrue(wasCalled, "Execute should invoke the provided action.");
        }

        [TestMethod]
        public void CanExecuteChanged_Event_CanBeSubscribedAndUnsubscribed()
        {
            // Arrange
            var command = new RelayCommand(() => { });
            bool eventFired = false;
            EventHandler handler = (s, e) => eventFired = true;

            // Act
            command.CanExecuteChanged += handler;
            command.CanExecuteChanged -= handler;

            // The event is never fired by RelayCommand, but this test ensures no exception is thrown on add/remove
            Assert.IsFalse(eventFired, "Event should not fire by default.");
        }
    }
}
