using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SCONT.Dominio.Entidades.Sistema;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Dominio.Contratos
{

    /// <summary>
    /// Nombre       : IrecursoRepository
    /// Descripción  : Métodos de trabajo para IrecursoRepository
    /// </summary>
    /// <remarks>
    /// Creacion     : 11/08/2017 STC.TOOL.WIN
    /// Modificación :  
    /// </remarks>
    public interface IrecursoRepository
    {

        List<recurso> SelectAll_recurso(recurso objdato);
        Parametro Select_fecha_servidor();
    }
}
