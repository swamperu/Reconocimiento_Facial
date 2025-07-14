var BusquedaDireccion = {
    Dato: null,
    Dialog: null,
    Configurar: function () {
        var strGridId = 'BusquedaDireccion';
        BusquedaDireccion.Dialog = $('#dialog' + strGridId);
        BusquedaDireccion.Dialog.modal({ keyboard: false, backdrop: 'static', show: false }).on('shown.bs.modal', function (e) {
            $('#tbl' + strGridId).jqGrid('setGridWidth', $('#cntBusquedaDireccion').width());
        });

        BusquedaDireccion.SetGrid();

        $("#BusquedaDireccion_Nombre").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("BusquedaDireccion_Nombre");

        $("#BusquedaDireccion_Consulta").click(function () {
            BusquedaDireccion.Buscar();
            return false;
        });
        $("#BusquedaDireccion_Nombre").keypress(function (e) {
            if (e.which == 13) {
                $("#tblBusquedaDireccion").trigger("reloadGrid");
            }
        });

        $(window).resize(function () {
            $('#tbl' + strGridId).jqGrid('setGridWidth', $('#cntBusquedaDireccion').width());
        });
    },
    SetGrid: function () {
        var strGridId = 'BusquedaDireccion';
        $("#tbl" + strGridId).jqGrid({
            datatype: function () {
                var parFiltro = PROYECTOMED.GetPaginationJQGrid('tbl' + strGridId);
                parFiltro.parametro1 = $("#BusquedaDireccion_Nombre").val();
                PROYECTOMED.CargarGridLoad(PROYECTOMED.UrlActionMethod('direccion', 'GetBandejaBusqueda'), 'tbl' + strGridId, { 'dato': parFiltro });
            },
            colNames: ['', 'DESCRIPCIÓN'],
            colModel: [
            { name: 'cod', index: 'cod', align: 'center', width: 1, sortable: false, hidden: true },
            { name: 'nom', index: 'nom', align: 'left', width: 2, sortable: true, hidden: false }
            ],
            rowNum: 20, rowList: [20], pager: '#pag' + strGridId, sortname: 'nom', viewrecords: false, sortorder: 'asc',
            height: 460, shrinkToFit: true, autowidth: true, shrinkToFit: true, rownumbers: true, reloadAfterEdit: false, reloadAfterSubmit: false, viewrecords: true, hidegrid: false, autoencode: true, caption: ''
        }).navGrid('#pag' + strGridId, { refresh: false, search: false, add: false, edit: false, del: false });
    },
    Buscar: function (callback) {
        $("#btnBusquedaDireccion_Aceptar").unbind('click');
        $("#btnBusquedaDireccion_Aceptar").click(function () {
            var datRow = PROYECTOMED.GetRowData('tblBusquedaDireccion');
            if (datRow == null) {
                PROYECTOMED.MostrarMensaje('Debe seleccionar un registro de la lista', 'Advertencia')
                return false;
            }

            BusquedaDireccion.Dato = datRow;

            if (typeof callback == 'function') {
                BusquedaDireccion.Dialog.modal("hide");
                callback.call(BusquedaDireccion.Dialog);
            }
            return false;
        });

        $("#btnBusquedaDireccion_Consulta").unbind('click');
        $("#btnBusquedaDireccion_Consulta").click(function () {
            $("#tblBusquedaDireccion").trigger("reloadGrid");
            return false;
        });

        $("#BusquedaDireccion_Nombre").val('');
        $("#tblBusquedaDireccion").trigger("reloadGrid");
        BusquedaDireccion.Dialog.modal('show');
    }
}