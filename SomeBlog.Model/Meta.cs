namespace SomeBlog.Model
{
    public class Meta : Core.ModelBase
    {
        public int BlogId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// TextArea = 1,
        /// InputText = 2,
        /// Checkbox = 3,
        /// DropdownList = 4
        /// </summary>
        public int InputType { get; set; }
    }
}
