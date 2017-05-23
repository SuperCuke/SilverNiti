using Ozzy.DomainModel;

namespace SilverNiti.Core.DomainEvents
{
    public class ContactFormMessageRecieved : IDomainEvent
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public int MessageId { get; set; }
    }
}
