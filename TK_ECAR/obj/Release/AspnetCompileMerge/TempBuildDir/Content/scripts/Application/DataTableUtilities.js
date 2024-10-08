var globalobjDataTable = null;
var globalnameDataTable = "";
var globalnumberOfPagesToShowGoTo = 0;
var globaltextToShow = "";
var globalNumberPageSelect = "";
var globalChangePage = false;

function SetGoToPage(nameDataTable, objDataTable, numberOfPagesToShowGoTo, textToShow) {

    //if (tableFlotaInfo.pages > 10) {

    //    $("#tableflota_info").append('<div>Ir a página....<select id="selectIrApagina"></select></div>');
    //    for (var r = 1; r <= tableFlotaInfo.pages; r++) {
    //        $('#selectIrApagina').append($('<option>', {
    //            value: r,
    //            text: r.toString()
    //        }));
    //    }
    //}
    //tableflota.page(5).draw(false);
    $("#divGoToPage").remove();
    $("#selectGoToPage").remove();

    globalnameDataTable = nameDataTable;
    globalobjDataTable = objDataTable;
    globalnumberOfPagesToShowGoTo = numberOfPagesToShowGoTo;
    globaltextToShow = textToShow;

    var _tableInfo = objDataTable.page.info();

    var _datatable_info = "#" + nameDataTable + "_info";

    if (!globalChangePage) {
        globalNumberPageSelect = 1;
    }

    if (_tableInfo.pages >= numberOfPagesToShowGoTo) {
        var divToAdd = '<div id="divGoToPage">' + textToShow + '<select id="selectGoToPage"></select></div>'
        $(_datatable_info).append(divToAdd);
        for (var r = 1; r <= _tableInfo.pages; r++) {
            $('#selectGoToPage').append($('<option>', {
                selected : (globalNumberPageSelect == r.toString() ? true : false),
                value: r,
                text: r.toString()
            }));
        }
    }

    initializeEvents();
}

function initializeEvents()
{
    $("#selectGoToPage").change(function () {
        var val = $("#selectGoToPage option:selected").val();
        globalNumberPageSelect = val;
        globalobjDataTable.page(Number(val) - 1).draw(false);
    });

    var _datatable = "#" + globalnameDataTable;
    $(_datatable).on('page.dt', function () {
        globalChangePage = true;
    });

    globalobjDataTable.on('draw', function () {
        if (globalChangePage) {
            SetGoToPage(globalnameDataTable, globalobjDataTable, globalnumberOfPagesToShowGoTo, globaltextToShow);
            globalChangePage = false;
        }
    });
}

function addButtonDataTable(nameDataTable, idButton, textoButton) {
    var boton = "#" + idButton;
    var elementAppend = '<a id="iddelboton" class="dt-button buttons-excel buttons-html5 btn btn-default export-btn" tabindex="0" aria-controls="nombretable"><span>textodelboton</span></a>';
    elementAppend = elementAppend.replace("iddelboton", idButton).replace("nombretable", nameDataTable).replace("textodelboton", textoButton);

    $(boton).remove();
    $('.dt-buttons').append(elementAppend);
}


//Formateo de columnas numéricas.
//jQuery.extend(jQuery.fn.dataTableExt.oSort, {
//    "formatted-num-pre": function (a) {
//        a = a.replace('.', '').replace(',', '.').replace('€', '');
//        a = (a === "-" || a === "") ? 0 : a.replace(/[^\d\-\.]/g, "");
//        return parseFloat(a);
//    },

//    "formatted-num-asc": function (a, b) {
//        return a - b;
//    },

//    "formatted-num-desc": function (a, b) {
//        return b - a;
//    }
//});