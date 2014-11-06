
function verifyJsonString(data) {
    var post_data;
    if (typeof (data) != "object") {
        try {
            post_data = JSON.parse(data);
        } catch (e) {
            post_data = data;
        }
    } else {
        post_data = data;
    }
    return post_data;
}
function dateAdd(strInterval, Number) {
    var dtTmp = new Date();
    switch (strInterval) {
        case 's': return new Date(Date.parse(dtTmp) + (1000 * Number));
        case 'n': return new Date(Date.parse(dtTmp) + (60000 * Number));
        case 'h': return new Date(Date.parse(dtTmp) + (3600000 * Number));
        case 'd': return new Date(Date.parse(dtTmp) + (86400000 * Number));
        case 'w': return new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));
        case 'q': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number * 3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'm': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'y': return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
    }
}
function trim(strVal) {
    if (isNullOrUndefined(strVal)) return;
    return strVal.replace(/(^\s*)|(\s*$)/g, "");
}
function isNullOrUndefined(val) {
    if (val == null || typeof (val) == "undefined") {
        return true;
    }
    return false;
}
function isNullOrTrimEmpty(strVal) {
    if (isNullOrUndefined(strVal)) return true;
    var tmp = trim(strVal);
    if (tmp.length <= 0) return true;
    return false;
}
//createCookie
function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}
//end Cookie

