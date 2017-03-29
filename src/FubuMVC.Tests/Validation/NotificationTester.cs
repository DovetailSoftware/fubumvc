using System.Collections.Generic;
using FubuCore.Reflection;
using FubuLocalization;
using FubuMVC.Core.Validation;
using FubuMVC.Tests.Validation.Models;
using FubuTestingSupport;
using NUnit.Framework;
using System.Linq;

namespace FubuMVC.Tests.Validation
{
    [TestFixture]
    public class NotificationTester
    {
        [Test]
        public void should_ignore_duplicates()
        {
            var notification = new Notification();
            notification.RegisterMessage<EntityToValidate>(e => e.Something, StringToken.FromKeyString("test"));
            notification.RegisterMessage<EntityToValidate>(e => e.Something, StringToken.FromKeyString("test"));
            notification.RegisterMessage<EntityToValidate>(e => e.Something, StringToken.FromKeyString("test"));
            notification.RegisterMessage<EntityToValidate>(e => e.Something, StringToken.FromKeyString("test"));
            notification.RegisterMessage<EntityToValidate>(e => e.Something, StringToken.FromKeyString("test"));

            notification
                .AllMessages
                .ShouldHaveCount(1);
        }

        [Test]
        public void valid_should_return_valid_notification()
        {
            var notification = new Notification();
            notification
                .IsValid()
                .ShouldBeTrue();
        }

        [Test]
        public void should_be_invalid_if_any_messages_are_registered()
        {
            var notification = new Notification();
            notification.RegisterMessage<EntityToValidate>(e => e.Something, StringToken.FromKeyString("test"));

            notification
                .IsValid()
                .ShouldBeFalse();
        }

        [Test]
        public void should_return_registered_messages()
        {
            var notification = new Notification();
            notification.RegisterMessage<EntityToValidate>(e => e.Something, StringToken.FromKeyString("test1"));
            notification.RegisterMessage<EntityToValidate>(e => e.Something, StringToken.FromKeyString("test2"));
            notification.RegisterMessage<EntityToValidate>(e => e.Something, StringToken.FromKeyString("test3"));


            notification
                .MessagesFor<EntityToValidate>(e => e.Something)
                .ShouldHaveCount(3);
        }

        [Test]
        public void to_validation_error_simple()
        {
            var notification = new Notification();
            notification.RegisterMessage<EntityToValidate>(e => e.Something, StringToken.FromKeyString("test1", "test1"));
            notification.RegisterMessage<EntityToValidate>(e => e.Something, StringToken.FromKeyString("test2", "test2"));
            notification.RegisterMessage<EntityToValidate>(e => e.Something, StringToken.FromKeyString("test3", "test3"));

            var errors = notification.ToValidationErrors();
            errors.Count().ShouldEqual(3);
            errors.First().message.ShouldEqual("test1");
            errors.First().field.ShouldEqual("Something");

        }

        [Test]
        public void to_validation_error_when_an_error_is_registered_without_an_accessor()
        {
            var notification = new Notification();
            notification.RegisterMessage(StringToken.FromKeyString("test1", "test1"));

            var error = notification.ToValidationErrors().Single();
            error.message.ShouldEqual("test1");
            error.field.ShouldBeEmpty();
        }

        [Test]
        public void to_validation_error_with_localization()
        {
            LocalizationManager.Stub();

            var notification = new Notification();
            notification.RegisterMessage<EntityToValidate>(e => e.Something, StringToken.FromKeyString("test1", "test1"));

            var errors = notification.ToValidationErrors();
            errors.First().label.ShouldEqual("en-US_Something");
        }

        [Test]
        public void to_validation_error_if_multiple_accessors_match_a_message()
        {
            var notification = new Notification();
            var token = StringToken.FromKeyString("test1", "test1");
            var message = new NotificationMessage(token);
            message.AddAccessor(ReflectionHelper.GetAccessor<EntityToValidate>(x => x.Something));
            message.AddAccessor(ReflectionHelper.GetAccessor<EntityToValidate>(x => x.Else));

            notification.RegisterMessage(message);

            var errors = notification.ToValidationErrors();
            errors.Length.ShouldEqual(2);

            errors.Each(x => x.message.ShouldEqual("test1"));

            errors.First().field.ShouldEqual("Something");
            errors.Last().field.ShouldEqual("Else");
        }

        [Test]
        public void add_child()
        {
            var child = new Notification();
            child.RegisterMessage<ContactModel>(x => x.FirstName, ValidationKeys.Required);
            child.RegisterMessage<ContactModel>(x => x.LastName, ValidationKeys.Required);

            var notification = new Notification(typeof(CompositeModel));
            var property = ReflectionHelper.GetAccessor<CompositeModel>(x => x.Contact);

            notification.AddChild(property, child);

            notification.MessagesFor<CompositeModel>(x => x.Contact.FirstName).Single().StringToken.ShouldEqual(ValidationKeys.Required);
            notification.MessagesFor<CompositeModel>(x => x.Contact.LastName).Single().StringToken.ShouldEqual(ValidationKeys.Required);
        }

        [Test]
        public void registering_a_message_adds_the_default_field_template_value()
        {
            var accessor = ReflectionHelper.GetAccessor<EntityToValidate>(x => x.Something);
            var notification = new Notification();
            notification.RegisterMessage(accessor, new NotificationMessage(StringToken.FromKeyString("Test", "{field}")));

            var message = notification.MessagesFor(accessor).Single();
            message.GetMessage().ShouldEqual(LocalizationManager.GetText(accessor.InnerProperty));
        }

        #region Nested Type: EntityToValidate
        public class EntityToValidate
        {
            public string Something { get; set; }
            public string Else { get; set; }
            public EntityToValidate Child { get; set; }
        }
        #endregion
    }
}