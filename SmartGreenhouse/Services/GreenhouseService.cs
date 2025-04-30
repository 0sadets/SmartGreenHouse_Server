using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Services
{
    public class GreenhouseService : IGreenhouseService
    {
        private readonly IRepository<Greenhouse> _repository;
        public GreenhouseService(IRepository<Greenhouse> repository)
        {
            _repository = repository;
        }
        public void Create(Greenhouse entity)
        {
            _repository.Create(entity);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public IEnumerable<Greenhouse> GetAll()
        {
            throw new NotImplementedException();
        }

        public Greenhouse GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Save()
        {
            _repository.Save();
        }

        public void Update(Greenhouse entity)
        {
            _repository.Update(entity);
        }
    }
}
