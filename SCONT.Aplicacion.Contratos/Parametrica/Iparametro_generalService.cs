using System.Collections.Generic;
using System.ServiceModel;
using SCONT.Dominio.Entidades.Parametrica;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Aplicacion.Contratos
{

    /// <summary>
    /// Nombre       : Iparametro_generalService
    /// Descripción  : Métodos de trabajo para Iparametro_generalService
    /// </summary>
    /// <remarks>
    /// Creacion     : 25/10/2017 STC.TOOL.WIN
    /// Modificación :  
    /// </remarks>
    [ServiceContract]
    public interface Iparametro_generalService
    {
        [OperationContract]
        DatoRetorno<parametro_general> Insert_parametro_general(parametro_general dato);

        [OperationContract]
        DatoRetorno<parametro_general> Update_parametro_general(parametro_general dato);

        [OperationContract]
        DatoRetorno<parametro_general> Delete_parametro_general(parametro_general dato);

        [OperationContract]
        DatoRetorno<parametro_general> Select_parametro_general(parametro_general dato);

        [OperationContract]
        DatoRetorno<parametro_general> SelectAll_parametro_general();

        [OperationContract]
        DatoRetorno<JQGrid> SelectPaginated_parametro_general(Parametro dato);

        [OperationContract]
        DatoRetorno<parametro_general> SelAll_ejecutora();

        [OperationContract]
        DatoRetorno<parametro_general> SelAll_ejecutora_usuario_centro_costo(Parametro dato);

        [OperationContract]
        DatoRetorno<parametro_general> SelAll_tipo_proceso();

        [OperationContract]
        DatoRetorno<parametro_general> SelAll_tipo_proceso_ps();

        [OperationContract]
        DatoRetorno<parametro_general> SelAll_modalidad_compra_asp();

    }
}
