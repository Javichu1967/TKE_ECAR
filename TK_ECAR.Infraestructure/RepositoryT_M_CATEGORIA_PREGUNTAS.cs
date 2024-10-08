using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_CATEGORIA_PREGUNTAS : RepositoryBase<T_M_CATEGORIA_PREGUNTAS>, IRepositoryT_M_CATEGORIA_PREGUNTAS
    {
		public RepositoryT_M_CATEGORIA_PREGUNTAS(ModelEntities context)
            : base(context)
        {

        }
    }
}