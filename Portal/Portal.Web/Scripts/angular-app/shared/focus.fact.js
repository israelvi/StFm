(function() {
	"use strict";

	angular
        .module(window.constMainApp)
        .factory('focusFact', focusFact);

	focusFact.$inject = ["$timeout"];

	function focusFact($timeout) {
		return function(id) {
			$timeout(function() {
				var element = document.getElementById(id);
				if (element)
					element.focus();
			}, 600);
		};
	};
})();