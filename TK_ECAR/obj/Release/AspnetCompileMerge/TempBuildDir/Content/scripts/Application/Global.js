function stripHtml(idForm) {
    var element = "form#" + idForm + " input[type=text], textarea";
    var txt = "";
    $(element).each(function () {
        var input = $(this); // This is the jquery object of the input, do what you will
        txt = $(this).val();
        txt = "<p>" + txt + "</p>";
        $(this).val($(txt).text());
    });
}


function formateaNumero(valorAformatear, numDecimales) {
    var valorReturn = "";
    if (valorAformatear != 0) {
        valorAformatear = String(parseFloat(String(valorAformatear).replace(',', '.')).toFixed(numDecimales)) //Forzamos dos decimales y redondeamos
        valorAformatear = valorAformatear.replace('.', ','); //Volvemos a poner la coma decimal
        var posicionComaDecimal = valorAformatear.indexOf(",");
        var iteraciones = posicionComaDecimal == -1 ? valorAformatear.length - 1 : posicionComaDecimal - 1;
        var contador = 0;
        for (i = iteraciones; i >= 0; i--) {
            contador++;
            valorReturn = ((contador % 3 == 0 && i > 0) ? '.' : '') + valorAformatear.substr(i, 1) + valorReturn;
        }
        if (posicionComaDecimal != -1) {
            valorReturn = valorReturn + valorAformatear.substr(posicionComaDecimal, (valorAformatear.length - posicionComaDecimal));
        }
    }
    else {
        valorReturn = "0";
    }
    return valorReturn;
}

function isValidDate(controlNameOrValue, format) {
    var isValid = true;
    var fecha = null;
    try
    {
        if (controlNameOrValue.substr(0, 1) == "#") {
            fecha = moment(jQuery(controlNameOrValue).val(), format);
        }
        else {
            fecha = moment(controlNameOrValue, format);
        }
        if (fecha._d == "Invalid Date") {
            isValid = false;
        }
        //$.datepicker.parseDate(format, jQuery('#' + controlName).val(), null);
    }
    catch (error) {
        isValid = false;
    }

    return isValid;
}


function convertToDate(controlNameOrValue) {
    var from = null;
    if (controlNameOrValue.substr(0, 1) == "#") {
        from = jQuery(controlNameOrValue).val().split("/");
    }
    else {
        from = controlNameOrValue.split("/");
    }

    return new Date(from[2], from[1] - 1, from[0]);
}

function dayDiff(controlNameOrValue1, controlNameOrValue2) {
    if (isValidDate(controlNameOrValue1, "dd/MM/yyyy") && isValidDate(controlNameOrValue2, "dd/MM/yyyy")) {
        var fecha1 = convertToDate(controlNameOrValue1);
        var fecha2 = convertToDate(controlNameOrValue2);
        return Math.round((fecha2 - fecha1) / (1000 * 60 * 60 * 24));
    }
    else {
        return -1;
    }
}

//Limpia los errores del ValidationSummary
function LimpiaValidationSummary() {
    $("#divValidationSummaryPanel_body ul").empty();
}

function LimpiaValidationSummaryBis() {
    $("#divValidationSummaryPanel_bodyBis ul").empty();
}

function QuitaFormatoNumericoDouble(campo) {
    var valor = $("#" + campo).val();
    valor = valor.replace("€", "").replace(" ", "");
    var position = valor.indexOf(".");
    while (position > -1) {
        valor = valor.replace(".", "");
        position = valor.indexOf(".");
    }
    $("#" + campo).val(valor);

    //}
}

function DevuelveConPuntoDecimal(valor) {
    valor = valor.replace("€", "").replace("%", "").replace(" ", "");
    var position = valor.indexOf(".");
    while (position > -1) {
        valor = valor.replace(".", "");
        position = valor.indexOf(".");
    }
    valor = valor.replace(",", ".");
    return valor;
}

function DevuelveValorInt(valor) {
    valor = valor.replace("€", "").replace("%", "").replace(" ", "").replace(".", "").replace(",", "");
    return valor;
}


function DevuelveFechaToString(date) {
    var yr = date.getFullYear(),
    month = date.getMonth() < 10 ? '0' + date.getMonth() : date.getMonth(),
    day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate(),
    newDate = yr + '-' + month + '-' + day;
    return newDate;
}

function GetCurrentDateFormatYYYYMMDD() {
    var d = new Date();

    var month = d.getMonth() + 1;
    var day = d.getDate();

    return d.getFullYear() + (month < 10 ? '0' : '') + month + (day < 10 ? '0' : '') + day;

}

//Retorna true si excede el tamaño máximo
function ExcededFilesSizeUpload(formName)
{
    var element = "form#" + formName + " input[type=file]";

    var ttlSize = $().not("[type='file']").serialize().length;

    $(element).each(function () {
        if (this.files.length > 0) {
            ttlSize += this.files[0].size;
        }
    });

    //Comprobar el tamaño de los archivos
    var size  = (sizeRequest * 1024);
    if (ttlSize > (size)) {
        return true;
    }
    return false;
}