var settings = {
    validClass: "is-valid",
    errorClass: "is-invalid"
};

$.validator.setDefaults(settings);
$.validator.unobtrusive.options = settings;

$.validator.setDefaults({
    ignore: ":hidden:not(.force-validation), .ignore-validation"
});

var AppValidation = function () { };

AppValidation.is = function (value1, operator, value2, passOnNull) {
    if (passOnNull) {
        var isNullish = function (input) {
            return input == null || input == undefined || input == "";
        };

        var value1nullish = isNullish(value1);
        var value2nullish = isNullish(value2);

        if ((value1nullish && !value2nullish) || (value2nullish && !value1nullish))
            return true;
    }

    var isNumeric = function (input) {
        return !isNaN(parseFloat(input));
    };

    var isDate = function (input) {
        var dateTest = new RegExp(/(?=\d)^(?:(?!(?:10\D(?:0?[5-9]|1[0-4])\D(?:1582))|(?:0?9\D(?:0?[3-9]|1[0-3])\D(?:1752)))((?:0?[13578]|1[02])|(?:0?[469]|11)(?!\/31)(?!-31)(?!\.31)|(?:0?2(?=.?(?:(?:29.(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:(?:\d\d)(?:[02468][048]|[13579][26])(?!\x20BC))|(?:00(?:42|3[0369]|2[147]|1[258]|09)\x20BC))))))|(?:0?2(?=.(?:(?:\d\D)|(?:[01]\d)|(?:2[0-8])))))([-.\/])(0?[1-9]|[12]\d|3[01])\2(?!0000)((?=(?:00(?:4[0-5]|[0-3]?\d)\x20BC)|(?:\d{4}(?!\x20BC)))\d{4}(?:\x20BC)?)(?:$|(?=\x20\d)\x20))((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]\d|2[0-3])(?::[0-5]\d){1,2})?$/);

        return dateTest.test(input);
    };

    var isBool = function (input) {
        return input === true || input === false || input === "true" || input === "false";
    };

    var timeRegex = new RegExp(/(?=\d)^((?<days>\d+)\.)?(?<hours>[0-1]?\d|2[0-4]):(?<mins>[0-5]?\d)(:(?<secs>[0-5]?\d))?(\.(?<milis>\d{1,3}))?$/);
    var isTime = function (input) {
        return timeRegex.test(input);
    };
    var getTime = function (input) {
        var regexExec = timeRegex.exec(input);
        if (!regexExec)
            return NaN;

        var days = regexExec.groups["days"] || "0";
        var hours = regexExec.groups["hours"];
        var mins = regexExec.groups["mins"];
        var secs = regexExec.groups["secs"] || "0";
        var milis = regexExec.groups["milis"] || "0";
        return parseInt(days) * 24 * 3600 * 1000 // Days in milisecs
            + parseInt(hours) * 3600 * 1000      // Hours in milisecs
            + parseInt(mins) * 60 * 1000		 // Minutes in milisecs
            + parseInt(secs) * 1000			     // Seconds in milisecs
            + parseInt(milis);
    };

    if (isTime(value1)) {
        value1 = getTime(value1);
        value2 = getTime(value2);
    }
    else if (isDate(value1)) {
        value1 = Date.parse(value1);
        value2 = Date.parse(value2);
    }
    else if (isBool(value1)) {
        if (value1 == "false") value1 = false;
        if (value2 == "false") value2 = false;
        value1 = !!value1;
        value2 = !!value2;
    }
    else if (isNumeric(value1)) {
        value1 = parseFloat(value1);
        value2 = parseFloat(value2);
    }

    switch (operator) {
        case "EqualTo":
            if (value1 == value2) return true;
            break;
        case "NotEqualTo":
            if (value1 != value2) return true;
            break;
        case "GreaterThan":
            if (value1 > value2) return true;
            break;
        case "LessThan":
            if (value1 < value2) return true;
            break;
        case "GreaterThanOrEqualTo":
            if (value1 >= value2) return true;
            break;
        case "LessThanOrEqualTo":
            if (value1 <= value2) return true;
            break;
        case "RegExMatch":
            return (new RegExp(value2)).test(value1);
        case "NotRegExMatch":
            return !(new RegExp(value2)).test(value1);
        case "In":
            try {
                var valArr = JSON.parse(value2);
                if (typeof (valArr) == "object")
                    for (var key in valArr)
                        if (valArr[key] == value1)
                            return true;
            } catch (e) { }

            return value1 == value2;
        case "NotIn":
            try {
                var valArr = JSON.parse(value2);
                if (typeof (valArr) == "object")
                    for (var key in valArr)
                        if (valArr[key] == value1)
                            return false;
            } catch (e) { }

            return value1 != value2;
    }

    return false;
};

AppValidation.getId = function (element, dependentPropety) {
    var pos = element.id.lastIndexOf("_") + 1;
    return element.id.substr(0, pos) + dependentPropety.replace(/\./g, "_");
};

AppValidation.getName = function (element, dependentPropety) {
    var pos = element.name.lastIndexOf(".") + 1;
    return element.name.substr(0, pos) + dependentPropety;
};

AppValidation.registerValidators = function (jQuery) {

    jQuery.validator.addMethod("mustbetrue", function (value, element, param) {
        return element.checked;
    });

    jQuery.validator.addMethod("is", function (value, element, params) {
        var dependentProperty = AppValidation.getId(element, params["dependentproperty"]);
        var operator = params["operator"];
        var passOnNull = params["passonnull"];
        var dependentValue = document.getElementById(dependentProperty).value;

        if (AppValidation.is(value, operator, dependentValue, passOnNull))
            return true;

        return false;
    });

    jQuery.validator.addMethod("requiredif", function (value, element, params) {
        var dependentProperty = AppValidation.getName(element, params["dependentproperty"]);
        var dependentTestValue = params["dependentvalue"];
        var operator = params["operator"];
        var pattern = params["pattern"];
        var dependentPropertyElement = document.getElementsByName(dependentProperty);
        var dependentValue = null;

        if (dependentPropertyElement.length > 1) {
            for (var index = 0; index != dependentPropertyElement.length; index++)
                if (dependentPropertyElement[index]["checked"]) {
                    dependentValue = dependentPropertyElement[index].value;
                    break;
                }

            if (dependentValue == null)
                dependentValue = false
        } else {
            if ($(dependentPropertyElement).hasClass("select2-hidden-accessible")) {
                dependentValue = $(dependentPropertyElement).select2('data')[0].id;
            } else {
                dependentValue = dependentPropertyElement[0].value;
            }
        }

        if (AppValidation.is(dependentValue, operator, dependentTestValue)) {
            if (pattern == null) {
                if (value != null && value.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "")
                    return true;
            }
            else
                return (new RegExp(pattern)).test(value);
        }
        else
            return true;

        return false;
    });

    jQuery.validator.addMethod("requiredifempty", function (value, element, params) {
        var dependentProperty = AppValidation.getId(element, params["dependentproperty"]);
        var dependentValue = document.getElementById(dependentProperty).value;

        if (dependentValue == null || dependentValue.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') == "") {
            if (value != null && value.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "")
                return true;
        }
        else
            return true;

        return false;
    });

    jQuery.validator.addMethod("requiredifnotempty", function (value, element, params) {
        var dependentProperty = AppValidation.getId(element, params["dependentproperty"]);
        var dependentValue = document.getElementById(dependentProperty).value;

        if (dependentValue != null && dependentValue.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "") {
            if (value != null && value.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "")
                return true;
        }
        else
            return true;

        return false;
    });
};

(AppValidation.registerValidators)(jQuery);

var setValidationValues = function (options, ruleName, value) {
    options.rules[ruleName] = value;
    if (options.message) {
        options.messages[ruleName] = options.message;
    }
};

var $AppValidation = $.validator.unobtrusive;

$AppValidation.adapters.add("requiredif", ["dependentproperty", "dependentvalue", "operator", "pattern"], function (options) {
    var value = {
        dependentproperty: options.params.dependentproperty,
        dependentvalue: options.params.dependentvalue,
        operator: options.params.operator,
        pattern: options.params.pattern
    };
    setValidationValues(options, "requiredif", value);
});

$AppValidation.adapters.add("is", ["dependentproperty", "operator", "passonnull"], function (options) {
    setValidationValues(options, "is", {
        dependentproperty: options.params.dependentproperty,
        operator: options.params.operator,
        passonnull: options.params.passonnull
    });
});

$AppValidation.adapters.add("requiredifempty", ["dependentproperty"], function (options) {
    setValidationValues(options, "requiredifempty", {
        dependentproperty: options.params.dependentproperty
    });
});

$AppValidation.adapters.add("requiredifnotempty", ["dependentproperty"], function (options) {
    setValidationValues(options, "requiredifnotempty", {
        dependentproperty: options.params.dependentproperty
    });
});

$AppValidation.adapters.addBool("mustbetrue");


(function ($) {
    $.validator.addMethod('filesize', function (value, element, param) {
        if (this.optional(element) || !element.files)
            return true;

        var bytes = 0;
        for (var i = 0; i < element.files.length; i++) {
            bytes += element.files[i].size;
        }

        return bytes <= parseFloat(param);
    });
    $.validator.unobtrusive.adapters.add('filesize', ['max'], function (options) {
        options.rules['filesize'] = options.params.max;
        options.messages['filesize'] = options.message;
    });


    $.validator.addMethod('acceptfiles', function (value, element, param) {
        if (this.optional(element))
            return true;

        var params = param.split(',');
        var extensions = [].map.call(element.files, function (file) {
            var extension = file.name.split('.').pop();

            return extension == file.name ? null : '.' + extension;
        });

        for (var i = 0; i < extensions.length; i++) {
            if (params.indexOf(extensions[i]) < 0) {
                return false;
            }
        }

        return true;
    });

    $.validator.unobtrusive.adapters.add('acceptfiles', ['extensions'], function (options) {
        options.rules['acceptfiles'] = options.params.extensions;
        options.messages['acceptfiles'] = options.message;
    });

}(jQuery));

//$.validator.addMethod('filesize', function (value, element, param) {
//    if (this.optional(element) || !element.files)
//        return true;

//    var bytes = 0;
//    for (var i = 0; i < element.files.length; i++) {
//        bytes += element.files[i].size;
//    }

//    return bytes <= parseFloat(param);
//});

//$.validator.unobtrusive.adapters.add('filesize', ['max'], function (options) {
//    options.rules['filesize'] = options.params.max;
//    options.messages['filesize'] = options.message;
//});

//// Accept Files
//$.validator.addMethod('acceptfiles', function (value, element, param) {
//    if (this.optional(element))
//        return true;

//    var params = param.split(',');
//    var extensions = [].map.call(element.files, function (file) {
//        var extension = file.name.split('.').pop();

//        return extension == file.name ? null : '.' + extension;
//    });

//    for (var i = 0; i < extensions.length; i++) {
//        if (params.indexOf(extensions[i]) < 0) {
//            return false;
//        }
//    }

//    return true;
//});

//$.validator.unobtrusive.adapters.add('acceptfiles', ['extensions'], function (options) {
//    options.rules['acceptfiles'] = options.params.extensions;
//    options.messages['acceptfiles'] = options.message;
//});
