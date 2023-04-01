namespace AspNetAngular.Entities
{
    public class Note
    {
        public Guid ApplicationUserId { get; set; }

        public Guid NoteId { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }
    }
}
