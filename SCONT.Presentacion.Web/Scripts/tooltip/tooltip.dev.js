$(document).tooltip({
	items: "[title],[data-content]",
	content: function () {
	    var element = $(this);
	    
	    if (element[0].classList[0] == "ribbon-button") {
	        if (element.is("[title]") && element.is("[data-content]")) {
	            return '<div style="font-size: 8pt;"><strong>' + element.attr("title") + '</strong><br />' + element.attr("data-content") + "</div>";
	        } else
	            return element.attr("title");
	    }

	}
});