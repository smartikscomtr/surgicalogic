namespace Surgicalogic.Model.InputModel
{
    public class PersonnelCategoryInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool SuitableForMultipleOperation { get; set; }
        public string Description { get; set; }
    }
}