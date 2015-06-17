(function () {
    "use strict";
    angular
        .module(window.constMainApp)
        .controller('modalDlgController', modalDlgController);

    modalDlgController.$inject = ["$scope", "$q", "sharedSvc"];

    function modalDlgController($scope, $q, sharedSvc) {
        var vm = this;
        vm.sharedSvc = sharedSvc;
        vm.showInnScope = showInnScope;
        vm.show = show;
        vm.doConfirm = doConfirm;
        vm.doConfirmFull = doConfirmFull;
        vm.doCancelDocument = doCancelDocument;
        vm.doObsolete = doObsolete;
        vm.doAction = doAction;
        vm.doPost = doPost;

        function showInnScope(data, urlToGo, divToAppendId, dlgUpsertId) {
            return vm.show(data, urlToGo, divToAppendId, dlgUpsertId, true);
        };

        function show(data, urlToGo, divToAppendId, dlgUpsertId, innerScp) {
            if (innerScp === true) { vm.working = true; } else { $scope.$apply(function () { vm.working = true; }); }
            var def = $q.defer();
            divToAppendId = divToAppendId || "#dlgUpsert";
            dlgUpsertId = dlgUpsertId || "#dlgUpModalId";

            var settings = {
                dataType: "html",
                type: "POST",
                url: urlToGo,
                data: data,
                success: function (resp) {
                    $(divToAppendId).empty().append(resp);
                    if (dlgUpsertId != undefined) {
                        var scope = angular.element($(dlgUpsertId)).scope();
                        scope.up.Model.def = def;
                        scope.$apply();
                    }
                    $scope.$apply(function () { vm.working = false; });
                },
                error: function () {
                    vm.sharedSvc.showMsg(
                        {
                            title: "Error de red",
                            message: "<strong>No fue posible conectarse al servidor</strong> <br/><br/>Por favor intente más tarde",
                            type: "danger"
                        }).then(function () { def.reject({ isError: true }); });
                    $scope.$apply(function () { vm.working = false; });
                }
            };
            $.ajax(settings);
            return def.promise;
        };

        function doConfirm(data, urlToGo, title, msg) {
            var def = $q.defer();
            vm.sharedSvc.showConf({ title: title, message: msg, type: "warning" }).
                then(function () {
                    vm.doPost(data, urlToGo, def);
                }, def.reject);
            return def.promise;
        };


        function doConfirmFull(data, urlToGo, title, message, type, choiceA) {
            var def = $q.defer();
            vm.sharedSvc.showConf({ title: title, message: message, type: type, choiceA: choiceA }).
                then(function (res) {
                    var dataToSend = data;
                    if (choiceA !== undefined) {
                        try {
                            dataToSend = choiceA.prepareData(data, res);
                        }
                        catch (e) {
                            dataToSend = data;
                        }
                    }
                    vm.doPost(dataToSend, urlToGo, def);
                }, def.reject);
            return def.promise;
        };

        function doCancelDocument(data, urlToGo, folio) {
            var def = $q.defer();
            vm.sharedSvc.showConf({ title: "Confirmación de cancelación de documento", message: "¿Está seguro que desea cancelar el documento con folio " + folio + "?", type: "warning" }).
                then(function () {
                    vm.doPost(data, urlToGo, def);
                }, def.reject);
            return def.promise;
        };

        function doObsolete(data, urlToGo) {
            var def = $q.defer();
            vm.sharedSvc.showConf({ title: "Eliminar registro", message: "¿Está seguro de que desea eliminar el registro?", type: "danger" }).
                then(function () {
                    vm.doPost(data, urlToGo, def);
                }, def.reject);
            return def.promise;
        };


        function doAction(data, urlToGo, title, message, type) {
            if (type == undefined)
                type = "danger";

            var def = $q.defer();
            vm.sharedSvc.showConf({ title: title, message: message, type: type }).
                then(function () {
                    vm.doPost(data, urlToGo, def);
                }, def.reject);
            return def.promise;
        };

        function doPost(data, urlToGo, def) {
            var settings = {
                dataType: "json",
                type: "POST",
                url: urlToGo,
                data: data,
                success: function (resp) {
                    if (resp.HasError === true) {
                        vm.sharedSvc.showMsg(
                            {
                                title: resp.Title,
                                message: resp.Message,
                                type: "danger"
                            }).then(function () { def.reject({ isError: true }); });
                    }
                    else {
                        def.resolve();
                    }
                },
                error: function () {
                    sharedSvc.showMsg(
                        {
                            title: "Error de red",
                            message: "<strong>No fue posible conectarse al servidor</strong> <br/><br/>Por favor intente más tarde",
                            type: "danger"
                        }).then(function () { def.reject({ isError: true }); });
                }
            };

            $.ajax(settings);
        };
    };
})();

