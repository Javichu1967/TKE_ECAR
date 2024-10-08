var arrayLetras = new Array(44);
// La posición 23 nunca sale, está en el array para las posiciones posteriores a la 23.
arrayLetras = new Array("T", "R", "W", "A", "G", "M", "Y", "F", "P", "D", "X", "B", "N", "J", "Z", "S", "Q",
                                   "V", "H", "L", "C", "K", "E", "T", "0", "9", "8", "7", "6", "5", "4", "3", "2", "1",
                                   "J", "I", "H", "G", "F", "E", "D", "C", "B", "A");

function fCalcularNIF_España(nif) {
    var correcto = true;
    var resto = 0;
    var dni;
    var letra;

    nif = nif.toUpperCase();
    if (nif.length < 9) // El DNI tiene que tener una logitud de 9 caracteres.
    {
        return false;
    }

    dni = nif.substring(0, nif.length - 1);
    // Comprueba que el dni está compuesto sólo por números
    for (i = 0; i < dni.length; i++) {
        ch = dni.charAt(i);
        if (isNaN(ch)) {
            return false;
        }
    }

    resto = dni % 23;

    if (arrayLetras[resto] == nif.charAt(nif.length - 1).toUpperCase()) {
        return true;
    }
    else {
        return false;
    }
}


// Funcion que comprueba que el parámetro pasado como valor es
// un NIE válido.
// nie: string que contiene el valor del NIE a validar
function fCalcularNIE_España(nie) {

    nie = nie.toUpperCase();
    //alert(nie);
    var letraX;
    var dni = 0;

    // Si el nie no tiene 3 caracteres o más salimos de la función
    if (nie.length < 9) {
        return false;
    }

    // Cogemos la primera posición del parametro
    letraX = nie.substring(0, 1);
    // Si la primera posicion del nie es una X o una K o una L o una M.
    if ((letraX = 'X') || (letraX = 'K') || (letraX = 'L') || (letraX = 'M')) {
        // Obtengo el número del nie.
        dni = nie.substring(1, nie.length - 1);
        // Comprueba que el dni está compuesto sólo por números
        for (i = 0; i < dni.length; i++) {
            ch = dni.charAt(i);
            if (isNaN(ch)) {
                return false;
            }
        }
    }
    else {
        return false;
    }
    resto = dni % 23;
    var letra = arrayLetras[resto];
    if (letra == nie.charAt(nie.length - 1).toUpperCase()) {
        return true;
    }
    else {
        return false;
    }

}

// Funcion que comprueba que el parámetro pasado como valor es
// un CIF válido.
// cif: string que contiene el valor del CIF a validar
function fCalcularCIF_España(cif) {

    cif = cif.toUpperCase();
    var acumulado = 0;
    var resto = 0;
    var valorFinal = 0;
    var codigoEntidad;
    var numero;
    var digitoControl;

    // Si el cif no tiene 3 caracteres o más salimos de la función
    if (cif.length < 9) {
        return false;
    }
    // Cogemos el primer caracter del CIF.
    codigoEntidad = cif.substring(0, 1);

    // Cogemos el número del cif que son todos los dígitos menos el primero y el último.
    numero = cif.substring(1, cif.length - 1);

    // Cogemos el último parámetro del CIF.
    digitoControl = cif.substring(cif.length - 1, cif.length);

    for (i = 0; i < numero.length; i++) {
        var ch = numero.charAt(i);
        if (isNaN(ch)) {
            // Si el caracter que estamos tratando no es un numero, no pasa la validación.
            return false;
        }
        if ((i + 1 == 1) || (i + 1 == 3) || (i + 1 == 5) || (i + 1 == 7)) {
            aux = ch * 2;
            if (aux > 9) {
                acumulado = acumulado + (aux % 10) + (aux / 10);
            }
            else {
                acumulado = parseInt(acumulado) + parseInt(aux);
            }
        }
        else {
            acumulado = parseInt(acumulado) + parseInt(ch);
        }
    }

    resto = parseInt(acumulado) % 10;
    if ((codigoEntidad == 'P') || (codigoEntidad == 'Q') || (codigoEntidad == 'S')) //|| (codigoEntidad=='Z') )
    {
        valor = resto + 34;
    }
        /*    
        else if ( (codigoEntidad=='A') && isNaN(digitoControl) )
        {
        if ( confirm('Está dando el alta de una empresa extranjera, ¿desea continuar?') )
        {
        valor = resto + 34;
        }
        else
        {
        return false;
        }
        }
        */
    else {
        valor = resto + 24;
    }

    var letra = arrayLetras[valor];
    if (letra == digitoControl) {
        return true;
    }
    else {
        return false;
    }
}

function fCalcularCIE_España(cie) {
    arrayLetrasCIF = new Array("A", "B", "C", "D", "E", "F", "G", "H", "I", "J");

    cie = cie.toUpperCase();
    var acumulado = 0;
    var resto = 0;
    var valorFinal = 0;
    var complemento = 0;
    var codigoEntidad;
    var numero;
    var digitoControl;

    // Cogemos el primer caracter del CIE.
    codigoEntidad = cie.substring(0, 1);

    // Cogemos el número del cie que son todos los dígitos menos el primero y el último.
    numero = cie.substring(1, cie.length - 1);

    // Cogemos el último parámetro del CIE.
    digitoControl = cie.substring(cie.length - 1, cie.length);

    for (i = 0; i < numero.length; i++) {
        var ch = numero.charAt(i);
        if (isNaN(ch)) {
            // Si el caracter que estamos tratando no es un numero, no pasa la validación.
            return false;
        }
        if ((i + 1 == 1) || (i + 1 == 3) || (i + 1 == 5) || (i + 1 == 7)) {
            aux = ch * 2;
            if (aux > 9) {
                // acumulado = acumulado + la ultima cifra de aux + la primera cifra de aux
                acumulado = acumulado + (aux % 10) + (aux / 10);
            }
            else {
                acumulado = parseInt(acumulado) + parseInt(aux);
            }
        }
        else {
            acumulado = parseInt(acumulado) + parseInt(ch);
        }
    }

    resto = parseInt(acumulado) % 10;
    complemento = 10 - resto;

    var letra = arrayLetrasCIF[complemento - 1];
    if (letra == digitoControl) {
        return true;
    }
    else {
        return false;
    }

}

function fCalcularDNI_España(dni) {
    if (dni.length != 8) // El DNI tiene que tener una logitud de 8 caracteres.
    {
        return false;
    }

    // Comprueba que el dni está compuesto sólo por números
    for (i = 0; i < dni.length; i++) {
        ch = dni.charAt(i);
        if (isNaN(ch)) {
            return false;
        }
    }

    return true;
}


function fvalidaDNI_Portugal(contribuinte) {
    // algoritmo de validação do NIF de acordo com
    // http://pt.wikipedia.org/wiki/N%C3%BAmero_de_identifica%C3%A7%C3%A3o_fiscal

    var temErro = 0;

    if (
    contribuinte.substr(0, 1) != '1' && // pessoa singular
    contribuinte.substr(0, 1) != '2' && // pessoa singular
    contribuinte.substr(0, 1) != '3' && // pessoa singular
    contribuinte.substr(0, 2) != '45' && // pessoa singular não residente
    contribuinte.substr(0, 1) != '5' && // pessoa colectiva
    contribuinte.substr(0, 1) != '6' && // administração pública
    contribuinte.substr(0, 2) != '70' && // herança indivisa
    contribuinte.substr(0, 2) != '71' && // pessoa colectiva não residente
    contribuinte.substr(0, 2) != '72' && // fundos de investimento
    contribuinte.substr(0, 2) != '77' && // atribuição oficiosa
    contribuinte.substr(0, 2) != '79' && // regime excepcional
    contribuinte.substr(0, 1) != '8' && // empresário em nome individual (extinto)
    contribuinte.substr(0, 2) != '90' && // condominios e sociedades irregulares
    contribuinte.substr(0, 2) != '91' && // condominios e sociedades irregulares
    contribuinte.substr(0, 2) != '98' && // não residentes
    contribuinte.substr(0, 2) != '99' // sociedades civis

    ) { temErro = 1; }
    var check1 = contribuinte.substr(0, 1) * 9;
    var check2 = contribuinte.substr(1, 1) * 8;
    var check3 = contribuinte.substr(2, 1) * 7;
    var check4 = contribuinte.substr(3, 1) * 6;
    var check5 = contribuinte.substr(4, 1) * 5;
    var check6 = contribuinte.substr(5, 1) * 4;
    var check7 = contribuinte.substr(6, 1) * 3;
    var check8 = contribuinte.substr(7, 1) * 2;

    var total = check1 + check2 + check3 + check4 + check5 + check6 + check7 + check8;
    var divisao = total / 11;
    var modulo11 = total - parseInt(divisao) * 11;
    if (modulo11 == 1 || modulo11 == 0) { comparador = 0; } // excepção
    else { comparador = 11 - modulo11; }


    var ultimoDigito = contribuinte.substr(8, 1) * 1;
    if (ultimoDigito != comparador) { temErro = 1; }

    if (temErro == 1) {
        return false;//alert('O numero de contribuinte parece estar errado');
    }
    else {
        return true;
    }

}