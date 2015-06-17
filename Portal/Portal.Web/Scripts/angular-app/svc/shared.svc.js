(function () {
    "use strict";
    angular
        .module(window.constMainApp)
        .service('sharedSvc', sharedSvc);

    sharedSvc.$inject = ["$timeout", "$q"];

    function sharedSvc($timeout, $q) {
        var vm = this;
        var dlgProcessing = $('#ProcessingDlgId');
        //Dialogo para mensajes con acciones de éxito, información, advertencia o error
        var dlgMsgBox = $('#MessageBoxDlgId');
        vm.cfgProc = { toProcessing: undefined, procCount: 0 };
        vm.cfgMsg = { Title: "", Message: "", Type: "" };
        vm.respMsg = {};

        vm.initCatalog = initCatalog;
        vm.onProcTimeOut = onProcTimeOut;
        vm.showProcessing = showProcessing;
        vm.hideProcessing = hideProcessing;
        vm.showDlg = showDlg;
        vm.showMsg = showMsg;
        vm.showConf = showConf;
        vm.hideMsg = hideMsg;

        function initCatalog(lstCatalog, m, keyItem, keyId) {
            var item = lstCatalog[0];

            if (m[keyId] !== undefined && m[keyId] > 0) {
                item = undefined;
                for (var i = 0; i < lstCatalog.length; i++) {
                    var itemCat = lstCatalog[i];
                    if (itemCat.Key === m[keyId]) {
                        item = itemCat;
                        break;
                    }
                }

                if (item === undefined) {
                    item = lstCatalog[0];
                }
            }

            m[keyId] = item.Key;
            m[keyItem] = item;
        };


        function onProcTimeOut() {
            vm.cfgProc.procCount++;
            if (vm.cfgProc.procCount > 100)
                vm.cfgProc.procCount = 0;
            vm.cfgProc.toProcessing = $timeout(vm.onProcTimeOut, 150);
        };

        function showProcessing() {
            dlgProcessing.modal('show');
            vm.cfgProc.procCount = 1;
            vm.cfgProc.toProcessing = $timeout(vm.onProcTimeOut, 400);
        };

        function hideProcessing() {
            $timeout.cancel(vm.cfgProc.toProcessing);
            dlgProcessing.modal('hide');
        };

        function showDlg(cfg) {
            vm.cfgMsg = cfg;
            var def = $q.defer();

            $timeout(function () {
                dlgMsgBox.modal('show');
                dlgMsgBox.on('hidden.bs.modal', function () {
                    if (vm.respMsg.vm.IsOk === true) {
                        def.resolve();
                    }
                    else {
                        def.reject();
                    }
                });
            }, 1);

            return def.promise;
        };

        function showMsg(cfg) {
            dlgMsgBox = $('#MessageBoxDlgId');
            return vm.showDlg(cfg);
        };


        function showConf(cfg) {
            dlgMsgBox = $('#ConfirmationDlgId');
            return vm.showDlg(cfg);
        };

        function hideMsg(rMsg) {
            vm.respMsg = rMsg;
            dlgMsgBox.modal('hide');
        };

    }
})();

