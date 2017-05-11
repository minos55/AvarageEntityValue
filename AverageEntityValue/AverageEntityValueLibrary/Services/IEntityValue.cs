using System.Threading.Tasks;

namespace Nomnio.AverageEntityValue.Interfaces
{
    public interface IEntityValue
    {
        Task<AverageEntitiesPropertyValues> GetAverage();
    }
}
