using Portfolio.Domain.Enums;

namespace Portfolio.Domain.Dtos.Messages
{
    public class UpdateMessageStatusDto
    {
        public int Id { get; set; }

        public MessageStatus MessageStatus{ get; set; }
    }
}
