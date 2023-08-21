// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.


function equipesAdd(url, ids, ano) {
    $.ajax({
        type: "POST",
        url: url,
        data: {
            idEquipe: ids,
            anoTemporada: ano
        },
        success: function (result) {
            console.log("Equipes adicionadas");
            window.location.href = '/Admin/Equipes/VisualizarEquipes?anoTemporada=' + ano
        },
        error: function (req, status, error) {
            console.log(status)
            console.log(req.responseText)
        }
    });
}

function equipesRemover(url, ids, ano) {
    $.ajax({
        type: "POST",
        url, url,
        data: {
            idEquipe: ids,
            anoTemporada: ano
        },
        success: function (result) {
            console.log("Equipes removidas");
            window.location.href = '/Admin/Equipes/VisualizarEquipes?anoTemporada='+ano
        },
        error: function (req, status, error) {
            console.log(status)
        }
    });
}

function pilotosAdd(url, ids, ano) {
    $.ajax({
        type: "POST",
        url: url,
        data: {
            idPiloto: ids,
            anoTemporada: ano
        },
        success: function (result) {
            console.log("Pilotos adicionados");
            window.location.href = '/Admin/Temporadas/VisualizarPilotosTemporada?anoTemporada=' + ano
        },
        error: function (req, status, error) {
            console.log(status)
            console.log(req.responseText)
        }
    });
}

function pilotosRemover(url, ids, ano) {
    $.ajax({
        type: "POST",
        url: url,
        data: {
            idPiloto: ids,
            anoTemporada: ano
        },
        success: function (result) {
            console.log("Pilotos adicionados");
            window.location.href = '/Admin/Temporadas/VisualizarPilotosTemporada?anoTemporada=' + ano
        },
        error: function (req, status, error) {
            console.log(status)
            console.log(req.responseText)
        }
    });
}

function pilotosEquipeAdd(url, ids, equipeId, ano) {
    $.ajax({
        type: "POST",
        url: url,
        data: {
            idPiloto: ids,
            equipeId: equipeId,
            anoTemporada: ano
        },
        success: function (result) {
            console.log("Pilotos adicionados");
            window.location.href = '/Admin/Equipes/AdicionarPilotosEquipe?anoTemporada=' + ano
        },
        error: function (req, status, error) {
            console.log(status)
            console.log(req.responseText)
        }
    })
}

function pilotosEquipeRemover(url, pilotoIds, equipeIds, ano) {
    $.ajax({
        type: "POST",
        url: url,
        data: {
            idPiloto: pilotoIds,
            idEquipe: equipeIds
        },
        success: function (result) {
            console.log("Pilotos adicionados");
            window.location.href = '/Admin/Equipes/AdicionarPilotosEquipe?anoTemporada=' + ano
        },
        error: function (req, status, error) {
            console.log(status)
            console.log(req.responseText)
        }
    })
}

function filtraCircuito(url, filtro) {
    $.ajax({
        type: "POST",
        url: url,
        data: {
            tipoCircuito: filtro
        },
        success: function (result) {
            var dropdown = $("#CircuitoId"); // Elemento da dropdownlist de circuitos
            dropdown.empty(); // Limpa os itens existentes

            // Preenche a dropdownlist com os circuitos filtrados
            for (var i = 0; i < result.length; i++) {
                var option = $("<option></option>")
                    .attr("value", result[i].circuitoId)
                    .text(result[i].nome);

                dropdown.append(option);
            }

            console.log("filtro selecionado: " + url);
            //window.location.href = '/Admin/Etapas/create?anoTemporada=' + ano
        },
        error: function (req, status, error) {
            console.log(status)
            console.log(req.responseText)
        }
    })
}