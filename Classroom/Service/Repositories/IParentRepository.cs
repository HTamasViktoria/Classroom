using Classroom.Model.DataModels;

namespace Classroom.Service.Repositories;

public interface IParentRepository
{
    IEnumerable<Parent> GetAllParents();
    Parent GetParentById(string parentId);
    IEnumerable<Parent> GetParentsByStudentId(string id);

}