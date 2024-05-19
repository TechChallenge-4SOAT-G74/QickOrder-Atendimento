using MongoDB.Driver;

namespace QuickOrderAtendimento.Infra.Gateway.Core
{
    public interface IMondoDBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
