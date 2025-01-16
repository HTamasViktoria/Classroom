using Classroom.Model.DataModels;
using Classroom.Service.Repositories;
using System;
using System.Collections.Generic;

namespace ClassromIntegrationTests.MockRepos
{
    public class MockParentRepository : IParentRepository
    {
        public IEnumerable<Parent> GetAllParents()
        {
            throw new Exception("Mock exception for testing.");
        }

        public Parent GetParentById(string id)
        {
            throw new Exception("Mock exception for testing.");
        }

        public IEnumerable<Parent> GetParentsByStudentId(string id)
        {
            throw new Exception("Mock exception for testing.");
        }

        public void AddParent(Parent parent)
        {
            throw new NotImplementedException();
        }

        public void UpdateParent(Parent parent)
        {
            throw new NotImplementedException();
        }

        public void DeleteParent(string id)
        {
            throw new NotImplementedException();
        }
    }
}