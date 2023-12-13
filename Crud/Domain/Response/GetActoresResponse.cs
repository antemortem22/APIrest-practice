using Crud.Domain.Entities;

namespace Crud.Domain.Response
{
    public class GetActoresResponse
    {
        public List<Actore> Actores { get; set; }
        public int Total {  get; set; }
    }
}
