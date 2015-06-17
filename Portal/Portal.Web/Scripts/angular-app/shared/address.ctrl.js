window.angJsDependencies.push('ui.bootstrap');
(function () {
    "use strict";
    angular
        .module(window.constMainApp)
        .controller('addressController', addressController);

    addressController.$inject = ['$http', 'focusFact', '$scope'];
    function addressController($http, focusFact, $scope) {
        var vm = this;
        vm.m = {};
        vm.zipCodes = zipCodes;
        vm.onSelect = onSelect;

        function zipCodes(val, url) {
            return $http.get(url, {
                params: {code: val}
            }).then(function (response) {
                try {
                    return response.data.map(function (item) {
                        item.ViewName = item.ZipCode + " (" + item.Location + ")";
                        return item;
                    });
                } catch (e) {
                    return [];
                } 
            });
        };

        function onSelect(idElement) {
            vm.m.ZipCode = vm.zipcode.ZipCode;
            vm.m.State = vm.zipcode.State;
            vm.m.Municipality = vm.zipcode.Municipality;
            vm.m.Location = vm.zipcode.Location;
            focusFact(idElement);
        };

        $scope.$on('onClearData', function () {
            vm.m = {};
            vm.zipcode = undefined;
        });

        $scope.$on('onSubsidiaryUpdate', function (event, res) {
            console.log(res.Data.Address);
            vm.m = res.Data.Address;
            vm.zipcode = res.Data.Address.ZipCode;
        });
    }
})();

