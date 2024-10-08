
function ConcatenaValoresChosen(id) {
    var elemnt = "#" + id;
    var valores = $(elemnt).val();

    var returnValue = "";

    $.each(valores, function (i, val) {
        returnValue = returnValue + (returnValue == "" ? "" : "-") + val + "";
    });

    return returnValue;
}


function LimpiaFiltroChosen(id) {
    var elemnt = "#" + id;
    $(elemnt).empty();
    $(elemnt).val('').trigger('chosen:updated');
    $(elemnt).chosen("destroy");
}


//Si duplica el objeto chosen (empresa, ...), con esta función deja sólo uno.
//Se le añadirá "_chosen" al id
function ArreglaChosen(id) {
                      //'div[id*="miIDModelo_chosen"]'
    var elementDiv = 'div[id*="' + id + "_chosen" + '"]';
    var elemnt = "[id=" + id + "_chosen" + "]";
    if ($(elementDiv).length > 1) {
        $(elemnt).each(function () {
            if (this.style.width != "0px") {
                this.remove();
            }
            if (this.style.width == "0px") {
                this.style.width = "100%";
            }
        });
    }
}

//Pone el aspa de borrar elemento en el chosen, del elemento seleccionado.
//Después de hacer un destroy;
//Se le añadirá "_chosen" al id
function PonerDeleteEnChosen(id) {
    var element = "#" + id  + "_chosen";
    var container = $(element);
    var selected_item = container.find('.chosen-single').first();
    var span_selected_item = selected_item.find("span").first();
    var abbr = '<abbr class="search-choice-close"></abbr>';
    span_selected_item.after(abbr);
}



