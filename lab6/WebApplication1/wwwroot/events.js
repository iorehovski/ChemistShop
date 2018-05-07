$("form").submit(function (e) {
    e.preventDefault();
    var id = this.elements["id"].value;
    var name = this.elements["medName"].value;
    var manufacturer = this.elements["manufacturer"].value;
    var storage = this.elements["storage"].value;

    if (id == 0) {
        AddMedicament(name, manufacturer, storage);
    }
    else {
        EditMedicament(id, name, manufacturer, storage);
    }
});

//изменение 
$("body").on("click", ".editLink", function () {
    var id = $(this).data("id");
    getMedicament(id);
});

//удаление
$("body").on("click", ".removeLink", function () {
    var id = $(this).data("id");
    DeleteMedicament(id);
});

function getMedicaments() {
    $.ajax({
        url: "/api/Home",
        type: "GET",
        contentType: "application/json",
        success: function (medicaments) {
            var res = "";
            $.each(medicaments, function (index, med) {
                res += generateRow(med);
            });

            $("table tbody").append(res);
        } 
    });
};

function getMedicament(id) {
    $.ajax({
        url: "/api/Home/" + id,
        type: "GET",
        contentType: "application/json",
        success: function (medicament) {
            var form = document.forms["medicament"];
            form.elements["id"].value = medicament.id;
            form.elements["medName"].value = medicament.medicamentName;
            form.elements["manufacturer"].value = medicament.manufacturer;
            form.elements["storage"].value = medicament.storage;
        }  
    });
};

function generateRow(item) {
    var row = "";

    row += "<tr data-rowid=\"" + item.id + "\"><td>" + item.id + "</td>" +
        "<td>" + item.medicamentName + "</td>" +
        "<td>" + item.manufacturer + "</td>" +
        "<td>" + item.storage + "</td>" +
        "<td><a class='editLink btn btn-success' data-id='" + item.id + "'>Изменить</a> &nbsp " +
        "<a class='removeLink btn btn-danger' data-id='" + item.id + "'>Удалить</a></td>" +
        "</tr>";

    return row;
}

$(document).ready(function () {
    getMedicaments();
});

$("#reset").click(function (e) {
    e.preventDefault();
    reset();
})

function reset() {
    $('#errors').html("");
    var form = document.forms["medicament"];
    form.elements["id"].value = 0;
    form.elements["medName"].value = "";
    form.elements["manufacturer"].value = "";
    form.elements["storage"].value = "";
}

function AddMedicament(name, manufacturer, storage) {
    $.ajax({
        url: "/api/Home/",
        contentType: "application/json",
        method: "POST",
        data: JSON.stringify({
            MedicamentName: name,
            Manufacturer: manufacturer,
            Storage: storage
        }),
        success: function (medicament) {
            reset();
            $("table tbody").append(generateRow(medicament));
        },
        error: function (jxqr, error, status) {
            // парсинг json-объекта
            if (jxqr.responseText === "") {
                $('#errors').append("<h3>" + jxqr.statusText + "</h3>");
            }
            else {
                var response = JSON.parse(jxqr.responseText);
                // добавляем общие ошибки модели
                if (response['']) {

                    $.each(response[''], function (index, item) {
                        $('#errors').append("<p>" + item + "</p>");
                    });
                }
            }
            $('#errors').show();
        }
    });
}

function EditMedicament(id, name, manufacturer, storage) {
    $.ajax({
        url: "/api/Home/" + id,
        contentType: "application/json",
        method: "PUT",
        data: JSON.stringify({
            Id: id,
            MedicamentName: name,
            Manufacturer: manufacturer,
            Storage: storage
        }),
        success: function (medicament) {
            reset();
            $("tr[data-rowid='" + medicament.id + "']").replaceWith(generateRow(medicament));
        },
        error: function (jxqr, error, status) {
            // парсинг json-объекта
            if (jxqr.responseText === "") {
                $('#errors').append("<h3>" + jxqr.statusText + "</h3>");
            }
            else {
                var response = JSON.parse(jxqr.responseText);
                // добавляем общие ошибки модели
                if (response['']) {

                    $.each(response[''], function (index, item) {
                        $('#errors').append("<p>" + item + "</p>");
                    });
                }
            }
            $('#errors').show();
        }
    });
}

function DeleteMedicament(id) {
    $.ajax({
        url: "/api/Home/" + id,
        contentType: "application/json",
        method: "DELETE",
        success: function (medicament) {
            $("tr[data-rowid='" + medicament.id + "']").remove();
        }
    })
}