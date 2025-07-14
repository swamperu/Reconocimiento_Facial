/// <reference path="../utils/proyectomed.js" />


$(document).ready(function () {
    //-----------inicial-------------
    iniProcess = true;
    var dialogReg = $('#dialogReg');
    dialogReg.modal({ keyboard: false, backdrop: 'static', show: false });
    $("[id$=txtFilNombre]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtFilNombre");
    $("[id$=txtRegCODIGO]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtRegCODIGO");
    $("[id$=txtRegNOMBRE]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtRegNOMBRE");
    PROYECTOMED.ConfigTab($("[name=idmenuset]").val());
    //-----------eventos-------------
    $("[id$=btnManConsulta]").click(function () {
        $('#tblData').trigger('reloadGrid');
        return false;
    });
    $("[id$=btnManNuevo]").click(function () {
        Registro.NewRegistro();
        return false;
    });
    $("[id$=btnManEditar]").click(function () {
        Registro.GetRegistro();
        return false;
    });
    $("[id$=btnManBorrar]").click(function () {
        Registro.DelRegistro();
        return false;
    });
    $("[id$=btnManInactivar]").click(function () {
        Registro.IncRegistro();
        return false;
    });
    $("[id$=btnManLimpiar]").click(function () {
        Registro.CleanFiltro();
        return false;
    });
    $("[id$=btnRegAceptar]").click(function () {
        Registro.UpdRegistro();
        return false;
    });
    //-----------control-------------
    var Registro = {
        Edicion: false,
        Codigo: null,
        //
        CleanFiltro: function () {
            $("[id*='txtFil']").val('');
            $("[id*='ddlFil']").val('');
            $('#tblData').trigger('reloadGrid');
        },
        //
        CleanRegistro: function () {
            $("[id*='txtReg']").val('');
            $("[id*='ddlReg']").val('');
            $(".form-group").removeClass("has-error");
            $("[id$=txtRegCODIGO]").prop("disabled", true);
        },
        //
        CheckRegistro: function () {
            if (Registro.Edicion) {
                if (!PROYECTOMED.CheckLength('txtRegCODIGO', 'Debe ingresar un valor en el campo CODIGO', 1))
                    return false;
            }
            if (!PROYECTOMED.CheckLength('txtRegNOMBRE', 'Debe ingresar un valor en el campo DESCRIPCÍÓN', 1))
                return false;

            return true;
        },
        //
        NewRegistro: function () {
            Registro.CleanRegistro();
            Registro.Edicion = false;
            Registro.Codigo = null;
            $("[id$=ddlRegACTIVO]").val('0');
            $("[id$=txtRegCODIGO]").val('AUTOGENERADO');
            $("[id$=ddlRegACTIVO]").prop('disabled', true);
            dialogReg.modal("show");
        },
        //
        GetRegistro: function () {
            var datRow = PROYECTOMED.GetRowData('tblData');
            if (datRow == null) {
                PROYECTOMED.MostrarMensaje('Debe seleccionar un registro de la lista', 'Advertencia')
                return false;
            }
            if (datRow.est == 'INACTIVO') {
                PROYECTOMED.MostrarMensaje('El registro está inactivo, no puede ser editado', 'Advertencia')
                return false;
            }
            var datParam = new Object();
            
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.ID_TABLA_GENERAL_DETALLE = datRow.reg;
            
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: PROYECTOMED.UrlActionMethod("GetRegistro"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                success: function (result) {
                    result = PROYECTOMED.GetResult(result);
                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        Registro.CleanRegistro();
                        Registro.Codigo = datTabla.ID_TABLA_GENERAL_DETALLE;
                        $("[id$=txtRegCODIGO]").val(datTabla.CODIGO);
                        $("[id$=txtRegNOMBRE]").val(datTabla.NOMBRE);
                        Registro.Edicion = true;
                        
                        if (msgTabla)
                            PROYECTOMED.MostrarMensaje(msgTabla, 'Suceso');
                        
                        dialogReg.modal('show');
                    } else {
                        PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }
                }
            });
        },
        //
        UpdRegistro: function () {
            if (!Registro.CheckRegistro())
                return false;
            
            var datParam = new Object();
            
            datParam.TOKEN = PROYECTOMED.GetToken();
            if(Registro.Edicion) datParam.ID_TABLA_GENERAL_DETALLE = Registro.Codigo;
            datParam.CODIGO = $("[id$=txtRegCODIGO]").val();
            datParam.NOMBRE = $("[id$=txtRegNOMBRE]").val();
            
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: PROYECTOMED.UrlActionMethod((Registro.Edicion ? "Upd" : "Ins") + "Registro"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                success: function (result) {
                    result = PROYECTOMED.GetResult(result);
                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        $("#tblData").trigger("reloadGrid");
                        dialogReg.modal('hide');
                        if (msgTabla)
                            PROYECTOMED.MostrarMensaje(msgTabla, 'Suceso');
                        
                    } else {
                        PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }
                }
            });
        },
        //
        IncRegistro: function () {
            var datRow = PROYECTOMED.GetRowData('tblData');
            if (datRow == null) {
                PROYECTOMED.MostrarMensaje('Debe seleccionar un registro de la lista', 'Advertencia')
                return false;
            }
            if (datRow.est == 'INACTIVO') {
                PROYECTOMED.MostrarMensaje('El registro ya está inactivo, verifique', 'Advertencia')
                return false;
            }
            var datParam = new Object();

            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.ID_TABLA_GENERAL_DETALLE = datRow.reg;

            PROYECTOMED.MostrarPopup('Inactivar', '¿Seguro de inactivar el registro?', 'SiNo', function () {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    url: PROYECTOMED.UrlActionMethod("IncRegistro"),
                    data: PROYECTOMED.SetParam({ 'dato': datParam }),
                    success: function (result) {
                        result = PROYECTOMED.GetResult(result);
                        var datTabla = result.Dato;
                        var msgTabla = result.Msg;
                        if (datTabla) {
                            $("#tblData").trigger("reloadGrid");
                            if (msgTabla)
                                PROYECTOMED.MostrarMensaje(msgTabla, 'Suceso');

                        } else {
                            PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                        }
                    }
                });
            });
        },
        //
        DelRegistro: function () {
            var datRow = PROYECTOMED.GetRowData('tblData');
            if (datRow == null) {
                PROYECTOMED.MostrarMensaje('Debe seleccionar un registro de la lista', 'Advertencia')
                return false;
            }
            var datParam = new Object();
            
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.ID_TABLA_GENERAL_DETALLE = datRow.reg;
            
            PROYECTOMED.MostrarPopup('Borrar', '¿Seguro de borrar el registro?', 'SiNo', function () {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    url: PROYECTOMED.UrlActionMethod("DelRegistro"),
                    data: PROYECTOMED.SetParam({ 'dato': datParam }),
                    success: function (result) {
                        result = PROYECTOMED.GetResult(result);
                        var datTabla = result.Dato;
                        var msgTabla = result.Msg;
                        if (datTabla) {
                            $("#tblData").trigger("reloadGrid");
                            if (msgTabla)
                                PROYECTOMED.MostrarMensaje(msgTabla, 'Suceso');
                            
                        } else {
                            PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                        }
                    }
                });
            });
        },
        //
    }
    //-----------grillas-------------
    $("#tblData").jqGrid({
        datatype: function () {
            var parFiltro = PROYECTOMED.GetPaginationJQGrid('tblData');
            parFiltro.PARAMETRO1 = $("#txtFilNombre").val();

            if (iniProcess) {
                PROYECTOMED.CargarGridLoad('GetBandeja', 'tblData', { 'dato': parFiltro });
            }
        },
        colNames: [
            '', 'CÓDIGO', 'DESCRIPCIÓN', 'ESTADO'],
        colModel: [
              { name: 'reg', index: 'reg', align: 'left', width: 1, sortable: false, hidden: true }
            , { name: 'cod', index: 'cod', align: 'center', width: 1, sortable: true, hidden: false }
            , { name: 'nom', index: 'nom', align: 'left', width: 7, sortable: true, hidden: false }
            , { name: 'est', index: 'est', align: 'left', width: 1, sortable: false, hidden: false }
        ],
        rowNum: 20, rowList: [20, 40, 50], pager: '#pagData', sortname: 'cod', viewrecords: false, sortorder: 'asc',
        height: 'auto', shrinkToFit: true, autowidth: true, shrinkToFit: true, rownumbers: true, reloadAfterEdit: false, reloadAfterSubmit: false, viewrecords: true, toolbar: [true, 'top'], hidegrid: false, autoencode: true, caption: 'LISTADO'
    }).navGrid('#pagData', { refresh: false, search: false, add: false, edit: false, del: false });

    $('#tblData').addToolBar('toolBarData');

    $(window).resize(function () {
        $('#tblData').jqGrid('setGridWidth', $('#contData').width());
    });
    iniProcess = true;
});
