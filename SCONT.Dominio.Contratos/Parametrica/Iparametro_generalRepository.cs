using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SCONT.Dominio.Entidades.Parametrica;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Dominio.Contratos
{

    /// <summary>
    /// Nombre       : Iparametro_generalRepository
    /// Descripción  : Métodos de trabajo para Iparametro_generalRepository
    /// </summary>
    /// <remarks>
    /// Creacion     : 25/10/2017 STC.TOOL.WIN
    /// Modificación :  
    /// </remarks>
    public interface Iparametro_generalRepository
    {
        bool Insert_parametro_general(parametro_general objdato);

        bool Update_parametro_general(parametro_general objdato);

        bool Delete_parametro_general(parametro_general objdato);

        parametro_general Select_parametro_general(parametro_general objdato);

        List<parametro_general> SelectAll_parametro_general();

        JQGrid SelectPaginated_parametro_general(Parametro objdato);

        List<parametro_general> SelAll_ejecutora();

        List<parametro_general> SelAll_ejecutora_usuario_centro_costo(Parametro objdato);

        List<parametro_general> SelAll_tipo_proceso();

        List<parametro_general> SelAll_tipo_proceso_ps();

        List<parametro_general> SelAll_modalidad_compra_asp();

    }
}

