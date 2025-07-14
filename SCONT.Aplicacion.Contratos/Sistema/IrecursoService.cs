using System.Collections.Generic;
using System.ServiceModel;
using SCONT.Dominio.Entidades.Sistema;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Aplicacion.Contratos
{

    /// <summary>
    /// Nombre       : IrecursoService
    /// Descripción  : Métodos de trabajo para IrecursoService
    /// </summary>
    /// <remarks>
    /// Creacion     : 11/08/2017 STC.TOOL.WIN
    /// Modificación :  
    /// </remarks>
    [ServiceContract]
    public interface IrecursoService
    {

        [OperationContract]
        DatoRetorno<recurso> SelectAll_recurso(recurso objdato);

    }
}
