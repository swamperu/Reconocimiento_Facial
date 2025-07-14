/// <reference path="../utils/ProyectoMED.js" />
$(document).ready(function () {
    iniProcess = false;

    //var tabs = $("[id*=ribbon-tab-header-0]");
    //var cods = tabs[0].id.substr(tabs[0].id.length - 4);
    //PROYECTOMED.ConfigTab(cods);
    $('.panel').tooltip({
        selector: "[data-toggle=tooltip]",
        container: "body"
    });

    //-----------eventos-------------
    $(".link_alerta").click(function () {
        var cod = $(this).attr("cod");
        Registro.ShowBandeja(cod);        
        return false;
    });

    $(".head_alerta").click(function () {
        var cod = $(this).attr("cod");
        Registro.ShowBandeja(cod);
        return false;
    });

    $(".btn_back").click(function () {
        $("#container_bandeja").hide();
        $("#container_alerta").show();
        return false;
    });

    $("#btnManConsulta").click(function () {
        Registro.GeneraReporte();
        return false;
    });

    //-----------funciones-------------
    var Registro = {
        Codigo: 0,
        //
        ShowBandeja: function (cod) {
            Registro.Codigo = cod;
            $('#tblData').jqGrid("clearGridData");

            $("#tblData").jqGrid('hideCol', ["mod", "cnt", "ent", "moe"]);
                        

            var titulo = "";
            switch (Registro.Codigo) {
                case "1":
                    titulo = "ÓRDENES DE BIENES/SERVICIOS ASP y OM SIN CRONOGRAMA";
                    $("#tblData").jqGrid('showCol', ["mod"]);
                    break;
                case "2":
                    titulo = "ÓRDENES DE BIENES/SERVICIOS ASP y OM QUE VENCEN HOY";
                    $("#tblData").jqGrid('showCol', ["cnt"]);
                    break;
                case "3":
                    titulo = "ÓRDENES DE BIENES/SERVICIOS ASP y OM QUE VENCEN EN LOS PRÓX. 7 DÍAS";
                    $("#tblData").jqGrid('showCol', ["cnt"]);
                    break;
                case "4":
                    titulo = "ÓRDENES DE SERVICIOS DE LOCADORES VIGENTES";
                    $("#tblData").jqGrid('showCol', ["cnt"]);
                    break;
                case "5":
                    titulo = "ENTREGABLES DE LAS ÓDENES DE BIENES/SERVICIOS ASP y OM QUE VENCEN HOY";
                    $("#tblData").jqGrid('showCol', ["ent", "moe"]);
                    break;
                case "6":
                    titulo = "ENTREGABLES DE LAS ÓRDENES DE BIENES/SERVICIOS ASP y OM QUE VENCEN EN LOS PRÓX. 7 DÍAS";
                    $("#tblData").jqGrid('showCol', ["ent", "moe"]);
                    break;
                default:
                    titulo = "";
            }

            $("#titulo_alerta").html(titulo);
            $('#tblData').trigger('reloadGrid');
            $("#tblData").jqGrid('setGridWidth', $("#contData").width());


            $("#container_bandeja").show();
            $("#container_alerta").hide();
            


        },
        //
        GeneraReporte: function () {
            var datParam = new Object();
            datParam.PARAMETRO15 = Registro.Codigo;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: PROYECTOMED.UrlActionMethod("GenerarReporte"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                success: function (result) {
                    result = PROYECTOMED.GetResult(result);
                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        window.open(PROYECTOMED.UrlActionMethod("reporte", datTabla), '_blank');
                    } else {
                        PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }
                },
            });

        },

    };


    //-----------grillas-------------
    $("#tblData").jqGrid({
        datatype: function () {
            var parFiltro = PROYECTOMED.GetPaginationJQGrid('tblData');
            if (iniProcess) {
                parFiltro.PARAMETRO15 = Registro.Codigo;
                PROYECTOMED.CargarGridLoad('GetBandeja', 'tblData', { 'dato': parFiltro });
            }
        },
        colNames: ['ID_ORDEN', 'AÑO', 'SEC_EJEC', 'TIPO<br>BIEN', 'N° ORDEN', 'FECHA<br>ORDEN', 'USUARIO<br>SIGA', 'EXP. SIAF', 'CONCEPTO', 'PROVEEDOR', 'MONTO (S/)', 'MODALIDAD', 'TIPO<br>CONTRATACIÓN', 'ÁREA USUARIA','ENTRE<BR>GABLE', 'MONTO ENTREGABLE'],
        colModel: [
            { name: 'cod', index: 'cod', align: 'center', width: 10, sortable: false, hidden: true },
            { name: 'ano', index: 'ano', align: 'center', width: 4, sortable: false, hidden: true },
            { name: 'sec', index: 'sec', align: 'left', width: 10, sortable: false, hidden: true },
            { name: 'tip', index: 'tip', align: 'center', width: 4, sortable: false, hidden: false },
            { name: 'nro', index: 'nro', align: 'center', width: 7, sortable: false, hidden: false },
            { name: 'fec', index: 'fec', align: 'center', width: 6, sortable: false, hidden: false },
            { name: 'usr', index: 'usr', align: 'left', width: 12, sortable: false, hidden: false },
            { name: 'exp', index: 'exp', align: 'center', width: 7, sortable: false, hidden: false },
            { name: 'con', index: 'con', align: 'left', width: 32, sortable: false, hidden: false },
            { name: 'prv', index: 'prv', align: 'left', width: 18, sortable: false, hidden: false },
            { name: 'mon', index: 'mon', align: 'right', width: 8, sortable: false, hidden: false, formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2 } },
            { name: 'mod', index: 'mod', align: 'left', width: 13, sortable: false, hidden: false },
            { name: 'cnt', index: 'cnt', align: 'left', width: 10, sortable: false, hidden: false },
            { name: 'are', index: 'are', align: 'left', width: 15, sortable: false, hidden: false },
            { name: 'ent', index: 'ent', align: 'center', width: 6, sortable: false, hidden: false },
            { name: 'moe', index: 'moe', align: 'right', width: 8, sortable: false, hidden: false, formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2 } },
        ],
        rowNum: 35,
        rowList: [12, 25, 35],
        pager: '#pagData',
        sortname: 'nro',
        sortorder: 'asc',
        height: 'auto',
        scrollOffset: 0,
        autowidth: true,
        shrinkToFit: true,
        rownumbers: true,
        reloadAfterEdit: false,
        reloadAfterSubmit: false,
        viewrecords: true,
        rownumWidth: 38,
        toolbar: [false, 'top'],
        hidegrid: false,
        autoencode: true,
        caption: 'LISTADO',
        gridComplete: function () {
            $('#tblData tr td').css("white-space", "normal");
        }
    }).navGrid('#pagData', { refresh: false, search: false, add: false, edit: false, del: false });

    $("#contData").resize(function () {
        $("#tblData").jqGrid('setGridWidth', $("#contData").width());
    });


    $(window).resize(function () {
        $('#tblData').jqGrid('setGridWidth', $('#contData').width());
    });

    $('#contData .ui-jqgrid .ui-jqgrid-bdiv').css('overflow-x', 'hidden');
    $('#contData .ui-jqgrid .ui-jqgrid-bdiv').css('overflow-y', 'hidden');

    iniProcess = true;

});