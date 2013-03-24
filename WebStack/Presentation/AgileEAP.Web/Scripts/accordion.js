/*
** AgileEAPCommerce custom accordion
*/

var Accordion = {
    checkAllow: false,
    disallowAccessToNextSections: false,
    sections: new Array(),
    currentSection: false,
    headers: new Array(),

    init: function (elem, clickableEntity, checkAllow) {
        this.checkAllow = checkAllow || false;
        this.disallowAccessToNextSections = false;
        this.sections = $('#' + elem + ' .section');
        this.currentSectionId = false;
        var headers = $('#' + elem + ' .section ' + clickableEntity);
        headers.click(function () {
            Accordion.headerClicked($(this));
        });
    },

    headerClicked: function (section) {
        Accordion.openSection(section.parent('.section'));
    },

    openSection: function (section) {
        var section = $(section);

        if (this.checkAllow && !section.hasClass('allow')) {
            return;
        }
        if (section.attr('id') != this.currentSectionId) {
            this.closeExistingSection();
            this.currentSectionId = section.attr('id');
            $('#' + this.currentSectionId).addClass('active');
            var contents = section.children('.a-item');
            $(contents[0]).show();


            if (this.disallowAccessToNextSections) {
                var pastCurrentSection = false;
                for (var i = 0; i < this.sections.length; i++) {
                    if (pastCurrentSection) {
                        $(this.sections[i]).removeClass('allow')
                    }
                    if ($(this.sections[i]).attr('id') == section.attr('id')) {
                        pastCurrentSection = true;
                    }
                }
            }
        }
    },

    closeSection: function (section) {
        var section = $(section);
        section.removeClass('active');
        var contents = section.children('.a-item');
        $(contents[0]).hide();
    },

    openNextSection: function (setAllow) {
        for (section in this.sections) {
            var nextIndex = parseInt(section) + 1;
            if (this.sections[section].id == this.currentSectionId && this.sections[nextIndex]) {
                if (setAllow) {
                    $(this.sections[nextIndex]).addClass('allow')
                }
                this.openSection(this.sections[nextIndex]);
                return;
            }
        }
    },

    openPrevSection: function (setAllow) {
        for (section in this.sections) {
            var prevIndex = parseInt(section) - 1;
            if (this.sections[section].id == this.currentSectionId && this.sections[prevIndex]) {
                if (setAllow) {
                    $(this.sections[prevIndex]).addClass('allow')
                }
                this.openSection(this.sections[prevIndex]);
                return;
            }
        }
    },

    closeExistingSection: function () {
        if (this.currentSectionId) {
            this.closeSection($('#' + this.currentSectionId));
        }
    }
};