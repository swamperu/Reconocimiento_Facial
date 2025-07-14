/// <reference path="../../utils/ProyectoMED.js" />

$(document).ready(function () {
    //-----------inicial-------------
    PROYECTOMED.ConfigTab($("[name=idmenuset]").val(), $("[name=idopcset]").val());
    iniProcess = false;
    var dialogRegParametro = $('#dialogRegParametro');
    dialogRegParametro.modal({ keyboard: false, backdrop: 'static', show: false }).on('shown.bs.modal', function (e) { });

    //$("[id$=txtParNOMBRE]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtParNOMBRE");
   // $("[id$=txtParVALOR]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtParVALOR");

    $("[id$=txtParCODIGO]").InputDisabled();

    //-----------eventos-------------
    $("[id$=btnManConsulta]").click(function () {
        $('#tblData').trigger('reloadGrid');
        return false;
    });

    $("[id$=btnManLimpiar]").click(function () {
        Registro.CleanFiltro();
    });

    $("[id$=btnManNuevo]").click(function () {
        Registro.NewParametro();
        return false;
    });

    $("[id$=btnParAceptar]").click(function () {
        Registro.UpdParametro();
        return false;
    });

    $('#tblData').delegate('.btnUsuEdit', 'click', function () {
        var id = $(this).attr("id");
        var id = id.replace("btnUsuEdit", "");
        $('#tblData').jqGrid('setSelection', parseInt(id), false);
        var ret = $("#tblData").getRowData(id);
        Registro.GetParametro(ret);
        return false;
    });

    $('#tblData').delegate('.btnUsuDel', 'click', function () {
        var id = $(this).attr("id");
        var id = id.replace("btnUsuDel", "");
        $('#tblData').jqGrid('setSelection', parseInt(id), false);
        var ret = $("#tblData").getRowData(id);
        Registro.DelParametro(ret);
        return false;
    });

    $("[id$=txtParVALOR]").keypress(function () {
        var max = $(this).attr('maxLength');
        var length = $(this).val().length;
        if (length > max) {
            $(this).val($(this).val().substr(0, max -1));
        }

    });

    //-----------funciones-------------
    var Registro = {
        CodigoParametro: 0,
        Edicion: false,
        //
        CleanFiltro: function () {
            $("[id*='txtFil']").val('');
            $("[id*='ddlFil']").val('');
            $("[id$='txtFilANIO_EJE']").val($("[id$='hfAnioActual']").val());
            $('#tblData').jqGrid("clearGridData");
        },
        //
        CleanParametro: function () {
            $("[id*='hfPar']").val('');
            $("[id*='txtPar']").val('');
            $("[id*='ddlPar']").val('');
            $(".form-group").removeClass("has-error");
        },
        //
        NewParametro: function () {
            Registro.CleanParametro();
            Registro.CodigoParametro = 0;
            Registro.Edicion = false;
            dialogRegParametro.modal("show");
        },
        //
        CheckParametro: function () {
            if (!PROYECTOMED.CheckLength('txtParCODIGO', 'Debe ingresar un valor en el campo CODIGO', 1))
                return false;
            if (!PROYECTOMED.CheckLength('txtParNOMBRE', 'Debe ingresar un valor en el campo NOMBRE', 1))
                return false;
            if (!PROYECTOMED.CheckLength('txtParVALOR', 'Debe ingresar un valor en el campo VALOR', 1))
                return false;
            return true;

        },
        //
        GetParametro: function (ret) {
            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.CODIGO = ret.c01;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: PROYECTOMED.UrlActionMethod("GetParametro"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                success: function (result) {
                    result = PROYECTOMED.GetResult(result);
                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        Registro.CleanParametro();
                        Registro.CodigoParametro = datTabla.ID_PARAMETRO_GENERAL;
                        Registro.Edicion = true;

                        $("[id$=txtParVALOR]").attr("maxlength", "5");

                        if (datTabla.CODIGO == "HORA_ENTRENAMIENTO") {
                            $("[id$=txtParVALOR]").attr("type", "time");
                        } else if (datTabla.CODIGO == "PORCENTAJE_CONFIANZA" || datTabla.CODIGO == "TOTAL_FOTO") {
                            $("[id$=txtParVALOR]").attr("type", "number");
                        } else {
                            $("[id$=txtParVALOR]").attr("type", "text");
                            $("[id$=txtParVALOR]").attr("maxlength", "250");
                        }

                        
                        $("[id$=txtParCODIGO]").val(datTabla.CODIGO);
                        $("[id$=txtParNOMBRE]").val(datTabla.NOMBRE);
                        $("[id$=txtParVALOR]").val(datTabla.VALOR);
                        
                        dialogRegParametro.modal("show");
                        if (msgTabla)
                            PROYECTOMED.MostrarMensaje(msgTabla, 'Suceso');
                    } else {
                        PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }
                }
            });
        },
        //
        UpdParametro: function () {
            if (!Registro.CheckParametro())
                return false;

            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.ID_PARAMETRO_GENERAL = Registro.CodigoParametro;
            datParam.NOMBRE = $("[id$=txtParNOMBRE]").val();
            datParam.VALOR = $("[id$=txtParVALOR]").val();
            
            
            //$.ajax({
            //    type: "POST",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    url: PROYECTOMED.UrlActionMethod((Registro.Edicion ? "Upd" : "Ins") + "Parametro"),
            //    data: PROYECTOMED.SetParam({ 'dato': datParam }),
            //    success: function (result) {
            //        result = PROYECTOMED.GetResult(result);
            //        var datTabla = result.Dato;
            //        var msgTabla = result.Msg;
            //        if (datTabla) {
            //            $("#tblData").trigger("reloadGrid");
            //            dialogRegParametro.modal("hide");
            //            if (msgTabla)
            //                PROYECTOMED.MostrarMensaje(msgTabla, 'Suceso');
            //        } else {
            //            PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
            //        }
            //    }
            //});

            PROYECTOMED.Ajax({
                url: PROYECTOMED.UrlActionMethod((Registro.Edicion ? "Upd" : "Ins") + "Parametro"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                tokenRequired: true,
                success: function (result) {

                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        $("#tblData").trigger("reloadGrid");
                        dialogRegParametro.modal("hide");
                        if (msgTabla)
                            PROYECTOMED.MostrarMensaje(msgTabla, 'Suceso');
                    } else {
                        PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }

                }
            });
        },
        //
        DelParametro: function (ret) {
            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.ID_USUARIO = ret.cod;

            PROYECTOMED.MostrarPopup('Borrar', '¿Seguro de borrar el registro?', 'SiNo', function () {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    url: PROYECTOMED.UrlActionMethod("DelParametro"),
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
    }
    //-----------grillas-------------
    $("#tblData").jqGrid({
        datatype: function () {
            var parFiltro = PROYECTOMED.GetPaginationJQGrid('tblData');
            PROYECTOMED.CargarGridLoad('GetBandeja', 'tblData', { 'dato': parFiltro });
        },
        colNames: ['ID_PARAMETRO', 'CÓDIGO', 'NOMBRE', 'VALOR','ACCIONES'],
        colModel: [
            { name: 'cod', index: 'cod', align: 'left', width: 5, sortable: false, hidden: true },
            { name: 'c01', index: 'c01', align: 'left', width: 15, sortable: false, hidden: false },
            { name: 'c02', index: 'c02', align: 'left', width: 22, sortable: false, hidden: false },
            { name: 'c03', index: 'c03', align: 'left', width: 15, sortable: false, hidden: false },
            { name: 'c09', index: 'c09', align: 'center', width: 12, sortable: false, hidden: false, formatter: VerAccionesParametro },
        ],
        rowNum: 15,
        rowList: [15, 30, 40],
        pager: '#pagData',
        sortname: 'c01',
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
        caption: 'LISTADO',
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

    function VerAccionesParametro(cellvalue, options, rowObject) {
        var html = "";
        html = "<div class='tooltip-demo'>";
        html = html + "<button class='btnUsuEdit btn btn-in-grid' type='button'  id='btnUsuEdit" + options.rowId + "' data-original-title='Editar parámetro' data-toggle='tooltip' data-placement='bottom'><i class='fa fa-pencil' ></i></button>";
        html = html + "</div>";
        return html;
    }

    $('#contData .ui-jqgrid .ui-jqgrid-bdiv').css('overflow-x', 'hidden');
    $('#contData .ui-jqgrid .ui-jqgrid-bdiv').css('overflow-y', 'hidden');
    iniProcess = true;
});
