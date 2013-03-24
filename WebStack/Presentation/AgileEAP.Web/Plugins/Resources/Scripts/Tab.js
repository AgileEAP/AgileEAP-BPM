function switchTab(curTab) {
    var tabs = $("#tabcontainer").find("li");
    var tabCount = tabs.size();
    var index = tabs.index($("#" + curTab));
    tabs.each(function (i) {
        if (this.id == curTab) {
            this.className = i == tabCount - 1 ? "tabBg7" : "tabBg1";
            $("#tab" + curTab).show();
        }
        else {
            $("#tab" + this.id).hide();
            if (i == 0) {
                this.className = "tabBg4";
            }
            else if (i + 1 == index) {
                this.className = "tabBg3";
            }
            else if (i == index + 1 && i == tabCount - 1) {
                this.className = "tabBg6";
            }
            else if (i == index + 1) {
                this.className = "tabBg2";
            } else if (i == tabCount - 1) {
                this.className = "tabBg5";
            }
            else {
                this.className = "tabBg3";
            }
        }
    });
}