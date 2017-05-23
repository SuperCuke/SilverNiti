using Ozzy;
using Ozzy.Server.EntityFramework;
using SilverNiti.Core.DomainEvents;

namespace SilverNiti.Core.Domain
{
    public class ContactFormMessage : AggregateBase<int>
    {
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public string Message { get; protected set; }
        public bool IsReplySent { get; protected set; }

        public ContactFormMessage(string name, string email, string message)
        {
            Guard.ArgumentNotNullOrEmptyString(name, nameof(name));
            Guard.ArgumentNotNullOrEmptyString(email, nameof(email));
            Guard.ArgumentNotNullOrEmptyString(message, nameof(message));

            Name = name;
            Email = email;
            Message = message;

            this.RaiseEvent(new ContactFormMessageRecieved
            {
                Name = Name,
                Email = email,
                Message = message,
                MessageId = Id
            });
        }
    }
}
