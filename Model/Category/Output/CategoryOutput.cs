using FluentValidation;
namespace Model.Category.Output
{
    public class CategoryOutput
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public int? parentId {get; set;}
        public bool HasSub { get; set; }=false;
    }
}