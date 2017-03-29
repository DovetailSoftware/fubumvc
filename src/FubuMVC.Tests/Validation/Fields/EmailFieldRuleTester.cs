using System.Linq;
using FubuMVC.Core.Validation;
using FubuMVC.Core.Validation.Fields;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Tests.Validation.Fields
{
    [TestFixture]
    public class EmailFieldRuleTester
    {
        private EmailTarget theTarget;

        [SetUp]
        public void SetUp()
        {
            theTarget = new EmailTarget();
        }

        private Notification theNotification
        {
            get
            {
                var scenario = ValidationScenario<EmailTarget>.For(x =>
                {
                    x.Model = theTarget;
                    x.FieldRule(new EmailFieldRule());
                });

                return scenario.Notification;
            }
        }

		[Test]
		public void default_token_is_email_key()
		{
			new EmailFieldRule().Token.ShouldEqual(ValidationKeys.Email);
		}

        [Test]
        public void no_message_if_email_is_valid()
        {
            theTarget.Email = "joel+paulus@arnold.com";
            theNotification.MessagesFor<EmailTarget>(x => x.Email).Any().ShouldBeFalse();
        }

        [Test]
        public void registers_message_if_email_is_invalid()
        {
            theTarget.Email = "something";
            var messages = theNotification.MessagesFor<EmailTarget>(x => x.Email);
            messages.Single().StringToken.ShouldEqual(ValidationKeys.Email);
        }

        public class EmailTarget
        {
            public string Email { get; set; }
        }
    }
}