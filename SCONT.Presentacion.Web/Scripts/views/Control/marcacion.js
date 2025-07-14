/// <reference path="../../utils/ProyectoMED.js" />

$(document).ready(function () {
    //-----------inicial-------------
    PROYECTOMED.ConfigTab($("[name=idmenuset]").val(), $("[name=idopcset]").val());
    $("[id*=txtFil]").prop("disabled", false);
    iniProcess = false;
    var dialogRegistro = $('#dialogRegistro');
    dialogRegistro.modal({ keyboard: false, backdrop: 'static', show: false }).on('shown.bs.modal', function (e) {
        PROYECTOMED.SetFormatPicker("dtpRegFECHA");
    });

    PROYECTOMED.SetFormatPicker("dtpFilFECHA_INICIO");
    PROYECTOMED.SetFormatPicker("dtpFilFECHA_FIN");
    $("[id$=txtFilDNI]").SoloNumerosEnteros();
    $("[id$=txtFilAPELLIDO_PATERNO]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtFilAPELLIDO_PATERNO");
    $("[id$=txtFilAPELLIDO_MATERNO]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtFilAPELLIDO_MATERNO");
    $("[id$=txtFilFECHA_INICIO]").SoloFecha();
    $("[id$=txtFilFECHA_FIN]").SoloFecha();

    $("[id$=txtRegFECHA]").SoloFecha();
    $("[id$=txtRegDNI]").SoloNumerosEnteros();


    //-----------eventos-------------
    $("[id$=btnManConsulta]").click(function () {
        if (!PROYECTOMED.CheckLength("txtFilFECHA_INICIO", "Debe ingresar una FECHA DESDE", 1))
            return false;
        if (!PROYECTOMED.isValidDate("txtFilFECHA_INICIO", "", true)) {
            PROYECTOMED.MostrarMensaje("Debe ingresar un valor correcto en el campo FECHA DESDE (DD/MM/YYYY)", "Error");
            return false;
        }
        if (!PROYECTOMED.CheckLength("txtFilFECHA_FIN", "Debe ingresar una FECHA HASTA", 1))
            return false;
        if (!PROYECTOMED.isValidDate("txtFilFECHA_FIN", "", true)) {
            PROYECTOMED.MostrarMensaje("Debe ingresar un valor correcto en el campo FECHA HASTA (DD/MM/YYYY)", "Error");
            return false;
        }
        $('#tblData').trigger('reloadGrid');
        return false;
    });

    $("[id$=btnManLimpiar]").click(function () {
        Registro.CleanFiltro();
    });

    $("[id$=btnManNuevo]").click(function () {
        Registro.NewRegistro();
        return false;
    });

    $("[id$=btnRegAceptar]").click(function () {
        Registro.UpdRegistro();
        return false;
    });

    $('#tblData').delegate('.btnRegDel', 'click', function () {
        var id = $(this).attr("id");
        var id = id.replace("btnRegDel", "");
        $('#tblData').jqGrid('setSelection', parseInt(id), false);
        var ret = $("#tblData").getRowData(id);
        Registro.DelRegistro(ret);
        return false;
    });
    
    //-----------funciones-------------
    var Registro = {
        Codigo: 0,
        Edicion: false,
        //
        CleanFiltro: function () {
            $("[id*='txtFil']").val('');
            $("[id*='ddlFil']").val('');
            $('#tblData').jqGrid("clearGridData");
        },
        //
        CleanRegistro: function () {
            $("[id*='hfReg']").val('');
            $("[id*='txtReg']").val('');
            $("[id*='ddlReg']").val('');
            PROYECTOMED.ClearCombo("ddlRegPROVINCIA", "(SELECCIONAR)");
            PROYECTOMED.ClearCombo("ddlRegDISTRITO", "(SELECCIONAR)");
            $(".form-group").removeClass("has-error");
        },
        //
        Bloquear: function (bloquear) {
            $("[id*=txtReg]").prop("disabled", bloquear);
            $("[id*=ddlReg]").prop("disabled", bloquear);
        },
        //
        NewRegistro: function () {
            Registro.CleanRegistro();
            Registro.Codigo = 0;
            Registro.Edicion = false;
            dialogRegistro.modal("show");
        },
        //
        CheckRegistro: function () {
            if (!PROYECTOMED.CheckLength('txtRegDNI', 'Debe ingresar un valor en el campo DNI', 1))
                return false;

            if (!PROYECTOMED.CheckLength('txtRegDNI', 'El campo DNI debe tener 8 dígitos', 8, 8))
                return false;

            if (!PROYECTOMED.CheckLength('ddlRegTIPO', 'Debe seleccionar un valor en el campo TIPO', 1))
                return false;

            if (!PROYECTOMED.CheckLength('txtRegFECHA', 'Debe ingresar un valor en el campo FECHA', 1))
                return false;

            if (!PROYECTOMED.CheckDate('txtRegFECHA', 'Debe ingresar un valor correcto en el campo FECHA (DD/MM/YYYY)'))
                return false;

            if (!PROYECTOMED.CheckLength('txtRegHORA', 'Debe ingresar un valor en el campo HORA - MINUTO', 1))
                return false;

            return true;

        },
        //
        UpdRegistro: function () {
            if (!Registro.CheckRegistro())
                return false;

            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.ID_MARCACION = Registro.Codigo;
            datParam.NUMERO_DNI = $("[id$=txtRegDNI]").val();
            datParam.FECHA_TEXTO = $("[id$=txtRegFECHA]").val();
            datParam.HORA_TEXTO = $("[id$=txtRegHORA]").val();
            datParam.INGRESO_SALIDA = $("[id$=ddlRegTIPO]").val();

            PROYECTOMED.Ajax({
                url: PROYECTOMED.UrlActionMethod((Registro.Edicion ? "Upd" : "Ins") + "Registro"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                tokenRequired: true,
                success: function (result) {
                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        $("#tblData").trigger("reloadGrid");
                        dialogRegistro.modal("hide");
                        if (msgTabla)
                            PROYECTOMED.MostrarMensaje(msgTabla, 'Suceso');
                    } else {
                        PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }
                }
            });

        },
        //
        DelRegistro: function (ret) {
            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.ID_MARCACION = ret.cod;

            PROYECTOMED.MostrarPopup('Borrar', '¿Seguro de borrar el registro?', 'SiNo', function () {
                PROYECTOMED.Ajax({
                    url: PROYECTOMED.UrlActionMethod("DelRegistro"),
                    data: PROYECTOMED.SetParam({ 'dato': datParam }),
                    tokenRequired: true,
                    success: function (result) {

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
    }
    //-----------grillas-------------
    $("#tblData").jqGrid({
        datatype: function () {
            var parFiltro = PROYECTOMED.GetPaginationJQGrid('tblData');
            parFiltro.PARAMETRO1 = $("#txtFilDNI").val();
            parFiltro.FECHA_DESDE = $("[id$=txtFilFECHA_INICIO]").val();
            parFiltro.FECHA_HASTA = $("[id$=txtFilFECHA_FIN]").val();

            PROYECTOMED.CargarGridLoad('GetBandeja', 'tblData', { 'dato': parFiltro });

        },
        colNames: ['ID_MARCACION', 'N° DNI', 'NOMBRES', 'APELLIDO PATERNO', 'APELLIDO MATERNO', 'TIPO', 'FECHA Y HORA MARCACIÓN', 'ACCIONES'],
        colModel: [
            { name: 'cod', index: 'cod', align: 'left', width: 5, sortable: false, hidden: true },
            { name: 'c02', index: 'c02', align: 'center', width: 10, sortable: false, hidden: false },
            { name: 'c04', index: 'c04', align: 'left', width: 14, sortable: false, hidden: false },
            { name: 'c05', index: 'c05', align: 'left', width: 14, sortable: false, hidden: false },
            { name: 'c06', index: 'c06', align: 'left', width: 14, sortable: false, hidden: false },
            { name: 'c07', index: 'c07', align: 'left', width: 10, sortable: false, hidden: false },
            { name: 'c08', index: 'c08', align: 'center', width: 10, sortable: false, hidden: false },
            { name: 'c09', index: 'c09', align: 'center', width: 12, sortable: false, hidden: false, formatter: VerAccionesRegistro },
        ],
        rowNum: 15,
        rowList: [15, 30, 40],
        pager: '#pagData',
        sortname: 'c03',
        sortorder: 'asc',
        height: 'auto',
        scrollOffset: 0,
        autowidth: true,
        shrinkToFit: true,
        rownumbers: true,
        reloadAfterEdit: false,
        reloadAfterSubmit: false,
        viewrecords: true,
        toolbar: [false, 'top'],
        hidegrid: false,
        autoencode: true,
        caption: '',
        gridComplete: function () {
            $('#tblData tr td').css("white-space", "normal");
            $('.tooltip-demo').tooltip({
                selector: "[data-toggle=tooltip]",
                container: "body"
            });
        },
    }).navGrid('#pagData', { refresh: false, search: false, add: false, edit: false, del: false });

    $("#contData").resize(function () {
        $("#tblData").jqGrid('setGridWidth', $("#contData").width());
    });

    $(window).resize(function () {
        $('#tblData').jqGrid('setGridWidth', $('#contData').width());
    });

    function VerAccionesRegistro(cellvalue, options, rowObject) {
        var html = "";
        html = "<div class='tooltip-demo'>";
        //html = html + "<button class='btnRegEdit btn btn-in-grid' type='button'  id='btnRegEdit" + options.rowId + "' data-original-title='Editar' data-toggle='tooltip' data-placement='bottom'><i class='fa fa-pencil' ></i></button>";
        html = html + "<button class='btnRegDel btn btn-in-grid' type='button'  id='btnRegDel" + options.rowId + "' data-original-title='Eliminar' data-toggle='tooltip' data-placement='bottom'><i class='fa fa-trash-o' ></i></button>";
        html = html + "</div>";
        return html;
    }

    $('#contData .ui-jqgrid .ui-jqgrid-bdiv').css('overflow-x', 'hidden');
    $('#contData .ui-jqgrid .ui-jqgrid-bdiv').css('overflow-y', 'hidden');

    iniProcess = true;

});
