(function() {
    "use strict";
    angular
        .module(window.constMainApp)
        .controller('upsertController', upsertController);

    upsertController.$inject = ["$scope", "$rootScope", "$sce", "notify"];

    function upsertController($scope, $rootScope, $sce, notify) {
        var vm = this;
        vm.WaitFor = false;
        vm.MsgError = "";
        vm.Model = {};
        vm.submit = submit;
        vm.fillSelect = fillSelect;
        vm.submitRedirect = submitRedirect;
        vm.returnUrl = returnUrl;
        vm.handleSuccessRedirect = handleSuccessRedirect;
        vm.handleSuccessWithId = handleSuccessWithId;
        vm.handleSuccess = handleSuccess;
        vm.handleError = handleError;
        vm.cancel = cancel;
        vm.setDlg = setDlg;
        vm.setReason = setReason;
        vm.formatHtml = formatHtml;
        vm.config = {isModal: true, hasNotify: false, hasNotifyError: false};

        function submit(formId, urlToPost, isValid, hasReturnId) {
            vm.MsgError = "";

            if (isValid !== undefined) {
                if (isValid !== true) {
                    vm.MsgError = $sce.trustAsHtml("Existe uno o más campos que no son válidos, son requeridos o su longitud está fuera de lo permitido");
                    vm.Invalid = true;
                    return false;
                }
            }

            if ($(formId).valid() === false) {
                vm.MsgError = $sce.trustAsHtml("Debe proporcionar toda la información para guardar");
                vm.Invalid = true;
                return false;
            }

            vm.WaitFor = true;

            if (hasReturnId === true) {
                $.post(urlToPost, $(formId).serialize())
                    .success(vm.handleSuccessWithId)
                    .error(vm.handleError);
            }
            else {
                $.post(urlToPost, $(formId).serialize())
                    .success(vm.handleSuccess)
                    .error(vm.handleError);
            }
            return true;
        };

        function fillSelect(model, list) {

            if (vm[list] === undefined || vm[list].length <= 0)
                return;

            if (vm[model] === undefined) {
                vm[model] = vm[list][0];
                $scope.$apply();
            }
        };

        function submitRedirect(formId, urlToPost, hasReturnId, validate) {

            var stVal = true;

            if (validate != undefined)
                stVal = validate();

            if ($(formId).valid() === false || stVal === false) {
                vm.Invalid = true;
                return false;
            }

            vm.WaitFor = true;

            if (hasReturnId === true) {
                $.post(urlToPost, $(formId).serialize())
                    .success(vm.handleSuccessWithId)
                    .error(vm.handleError);
            }
            else {
                $.post(urlToPost, $(formId).serialize())
                    .success(vm.handleSuccessRedirect)
                    .error(vm.handleError);
            }
            return true;
        };

        function returnUrl(urlToGo) {
            window.goToUrlMvcUrl(urlToGo, "");
        };


        function handleSuccessRedirect(resp) {
            vm.WaitFor = false;

            try {
                if (resp.HasError === undefined) {
                    resp = resp.responseMessage;
                }
                if (resp.HasError === false) {
                    window.goToUrlMvcUrl(resp.UrlToGo, "");
                    vm.WaitFor = false;
                    $scope.$apply();
                    return;
                }

                vm.MsgError = $sce.trustAsHtml(resp.Message);

                $scope.$apply();

            } catch (e) {
                vm.MsgError = $sce.trustAsHtml("Error inesperado de datos. Por favor intente más tarde.");
            }
        };

        function handleSuccessWithId(resp) {
            vm.WaitFor = false;

            try {
                if (resp.HasError === undefined) {
                    resp = resp.responseMessage;
                }
                if (resp.HasError === false) {
                    $rootScope.$broadcast("onLastId", resp.Id);
                    vm.Model.dlg.modal('hide');
                    vm.Model.def.resolve({ isCancel: false });
                    return;
                }

                vm.MsgError = $sce.trustAsHtml(resp.Message);
                $scope.$apply();

            } catch (e) {
                vm.MsgError = $sce.trustAsHtml("Error inesperado de datos. Por favor intente más tarde.");
            }
        };

        function handleSuccess(resp) {
            vm.WaitFor = false;

            try {

                if (resp.HasError === undefined) {
                    vm.handleError();
                }

                if (resp.HasError === false) {

                    if (vm.config.isModal === true) {
                        vm.Model.dlg.modal('hide');
                        vm.Model.def.resolve({ isCancel: false, resp: resp });
                    }

                    if (vm.config.hasNotify === true) {
                        notify({ message: resp.Message, classes: 'alert-info' });
                    }

                    $scope.$broadcast("onSuccessNotification", resp);

                    return;
                }

                vm.MsgError = $sce.trustAsHtml(resp.Message);

                if (vm.config.hasNotifyError === true) {
                    notify({ message: resp.Message, classes: 'alert-danger' });
                }

                $scope.$apply();

            } catch (e) {
                vm.MsgError = $sce.trustAsHtml("Error inesperado de datos. Por favor intente más tarde.");
                if (vm.config.hasNotifyError === true) {
                    notify({ message: resp.Message, classes: 'alert-danger' });
                }
                $scope.$apply();
            }
        };

        function handleError() {
            vm.WaitFor = false;
            vm.MsgError = $sce.trustAsHtml("Error de red. Por favor intente más tarde.");
            if (vm.config.hasNotifyError === true) {
                notify({ message: resp.Message, classes: 'alert-danger' });
            }
            $scope.$apply();
        };

        function cancel(isOk) {
            vm.Model.dlg.modal('hide');

            if (isOk === true)
                vm.Model.def.resolve({ isCancel: false });
            else
                vm.Model.def.reject({ isCancel: true });
        };

        function setDlg(dlg, urlToSubmit) {
            vm.Model.dlg = dlg;
            vm.Model.url = urlToSubmit;

            dlg.on('hidden.bs.modal', function () {
                dlg.data('modal', null);
                dlg.replaceWith("");
            });
        };

        function setReason(opc) {
            if (opc.isFinal && vm.Model.reason != "") {
                vm.Model.reason = opc.reason;
            }
        }

        function formatHtml(sHtml) {
            return $sce.trustAsHtml(sHtml);
        };

    }
})();

