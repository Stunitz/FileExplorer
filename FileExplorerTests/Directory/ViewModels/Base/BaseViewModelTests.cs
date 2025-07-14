using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using FileExplorer;

namespace FileExplorer.Tests
{
    [TestClass]
    public class BaseViewModelTests
    {
        private class TestViewModel : BaseViewModel
        {
            public void RaiseTestPropertyChanged(string propertyName)
            {
                // Use reflection to invoke the event since it's not accessible directly
                var eventInfo = typeof(BaseViewModel).GetField("PropertyChanged", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var handler = (PropertyChangedEventHandler)eventInfo.GetValue(this);
                handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [TestMethod]
        public void PropertyChanged_Event_CanBeSubscribedAndUnsubscribed()
        {
            // Arrange
            var vm = new TestViewModel();
            bool eventFired = false;
            PropertyChangedEventHandler handler = (s, e) => eventFired = true;

            // Act & Assert - Part 1: Verify event fires when subscribed
            vm.PropertyChanged += handler;
            vm.RaiseTestPropertyChanged("TestProperty");
            Assert.IsTrue(eventFired, "PropertyChanged event should fire when handler is subscribed.");

            // Act & Assert - Part 2: Verify event doesn't fire after unsubscribing
            vm.PropertyChanged -= handler;
            eventFired = false;
            vm.RaiseTestPropertyChanged("TestProperty2");
            Assert.IsFalse(eventFired, "PropertyChanged event should not fire after handler is unsubscribed.");
        }

        [TestMethod]
        public void PropertyChanged_Event_InitialDelegate_DoesNotThrow()
        {
            // Arrange
            var vm = new TestViewModel();
            // Act & Assert
            vm.RaiseTestPropertyChanged("AnyProperty"); // Should not throw
        }
    }
}
