/*!
* DevExtreme (dx.messages.cs.js)
* Version: 17.2.12
* Build date: Fri Feb 01 2019
*
* Copyright (c) 2012 - 2019 Developer Express Inc. ALL RIGHTS RESERVED
* Read about DevExtreme licensing here: https://js.devexpress.com/Licensing/
*/
"use strict";

! function(root, factory) {
    if ("function" === typeof define && define.amd) {
        define(function(require) {
            factory(require("devextreme/localization"))
        })
    } else {
        if ("object" === typeof module && module.exports) {
            factory(require("devextreme/localization"))
        } else {
            factory(DevExpress.localization)
        }
    }
}(this, function(localization) {
    localization.loadMessages({
        cs: {
            Yes: "Ano",
            No: "Ne",
            Cancel: "Zrušit",
            Clear: "Smazat",
            Done: "Hotovo",
            Loading: "Nahrávání...",
            Select: "Výběr...",
            Search: "Hledat",
            Back: "Zpět",
            OK: "OK",
            "dxCollectionWidget-noDataText": "Žádná data k zobrazení",
            "validation-required": "povinné",
            "validation-required-formatted": "{0} je povinných",
            "validation-numeric": "Hodnota musí být číslo",
            "validation-numeric-formatted": "{0} musí být číslo",
            "validation-range": "Hodnota je mimo rozsah",
            "validation-range-formatted": "{0} je mimo rozsah",
            "validation-stringLength": "Délka textového řetezce není správná",
            "validation-stringLength-formatted": "Délka textu {0} není správná",
            "validation-custom": "Neplatná hodnota",
            "validation-custom-formatted": "{0} je neplatných",
            "validation-compare": "Hodnoty se neshodují",
            "validation-compare-formatted": "{0} se neshoduje",
            "validation-pattern": "Hodnota neodpovídá vzoru",
            "validation-pattern-formatted": "{0} neodpovídá vzoru",
            "validation-email": "Neplatný email",
            "validation-email-formatted": "{0} není platný",
            "validation-mask": "Hodnota není platná",
            "dxLookup-searchPlaceholder": "Minimální počet znaků: {0}",
            "dxList-pullingDownText": "Stáhněte dolů pro obnovení...",
            "dxList-pulledDownText": "Uvolněte pro obnovení...",
            "dxList-refreshingText": "Obnovuji...",
            "dxList-pageLoadingText": "Nahrávám...",
            "dxList-nextButtonText": "Více",
            "dxList-selectAll": "Vybrat vše",
            "dxListEditDecorator-delete": "Smazat",
            "dxListEditDecorator-more": "Více",
            "dxScrollView-pullingDownText": "Stáhněte dolů pro obnovení...",
            "dxScrollView-pulledDownText": "Uvolněte pro obnovení...",
            "dxScrollView-refreshingText": "Obnovuji...",
            "dxScrollView-reachBottomText": "Nahrávám...",
            "dxDateBox-simulatedDataPickerTitleTime": "Vyberte čas",
            "dxDateBox-simulatedDataPickerTitleDate": "Vyberte datum",
            "dxDateBox-simulatedDataPickerTitleDateTime": "Vyberte datum a čas",
            "dxDateBox-validation-datetime": "Hodnota musí být datum nebo čas",
            "dxFileUploader-selectFile": "Vyberte soubor",
            "dxFileUploader-dropFile": "nebo přeneste soubor sem",
            "dxFileUploader-bytes": "bytů",
            "dxFileUploader-kb": "kb",
            "dxFileUploader-Mb": "Mb",
            "dxFileUploader-Gb": "Gb",
            "dxFileUploader-upload": "Nahrát",
            "dxFileUploader-uploaded": "Nahráno",
            "dxFileUploader-readyToUpload": "Připraveno k nahrání",
            "dxFileUploader-uploadFailedMessage": "Nahrávání selhalo",
            "dxRangeSlider-ariaFrom": "Od",
            "dxRangeSlider-ariaTill": "Do",
            "dxSwitch-onText": "ZAP",
            "dxSwitch-offText": "VYP",
            "dxForm-optionalMark": "volitelný",
            "dxForm-requiredMessage": "{0} je vyžadováno",
            "dxNumberBox-invalidValueMessage": "Hodnota musí být číslo",
            "dxDataGrid-columnChooserTitle": "Výběr sloupců",
            "dxDataGrid-columnChooserEmptyText": "Přesuňte sloupec zde pro skytí",
            "dxDataGrid-groupContinuesMessage": "Pokračovat na další straně",
            "dxDataGrid-groupContinuedMessage": "Pokračování z předchozí strany",
            "dxDataGrid-groupHeaderText": "Sloučit sloupce",
            "dxDataGrid-ungroupHeaderText": "Oddělit",
            "dxDataGrid-ungroupAllText": "Oddělit vše",
            "dxDataGrid-editingEditRow": "Upravit",
            "dxDataGrid-editingSaveRowChanges": "Uložit",
            "dxDataGrid-editingCancelRowChanges": "Zrušit",
            "dxDataGrid-editingDeleteRow": "Smazat",
            "dxDataGrid-editingUndeleteRow": "Obnovit",
            "dxDataGrid-editingConfirmDeleteMessage": "Opravdu chcete smazat tento záznam?",
            "dxDataGrid-validationCancelChanges": "Zrušit změny",
            "dxDataGrid-groupPanelEmptyText": "Přeneste hlavičku sloupce zde pro sloučení",
            "dxDataGrid-noDataText": "Žádná data",
            "dxDataGrid-searchPanelPlaceholder": "Hledání...",
            "dxDataGrid-filterRowShowAllText": "(Vše)",
            "dxDataGrid-filterRowResetOperationText": "Reset",
            "dxDataGrid-filterRowOperationEquals": "Rovná se",
            "dxDataGrid-filterRowOperationNotEquals": "Nerovná se",
            "dxDataGrid-filterRowOperationLess": "Menší",
            "dxDataGrid-filterRowOperationLessOrEquals": "Menší nebo rovno",
            "dxDataGrid-filterRowOperationGreater": "Větší",
            "dxDataGrid-filterRowOperationGreaterOrEquals": "Větší nebo rovno",
            "dxDataGrid-filterRowOperationStartsWith": "Začíná na",
            "dxDataGrid-filterRowOperationContains": "Obsahuje",
            "dxDataGrid-filterRowOperationNotContains": "Neobsahuje",
            "dxDataGrid-filterRowOperationEndsWith": "Končí na",
            "dxDataGrid-filterRowOperationBetween": "Mezi",
            "dxDataGrid-filterRowOperationBetweenStartText": "Začíná",
            "dxDataGrid-filterRowOperationBetweenEndText": "Končí",
            "dxDataGrid-applyFilterText": "Použít filtr",
            "dxDataGrid-trueText": "Platí",
            "dxDataGrid-falseText": "Neplatí",
            "dxDataGrid-sortingAscendingText": "Srovnat vzestupně",
            "dxDataGrid-sortingDescendingText": "Srovnat sestupně",
            "dxDataGrid-sortingClearText": "Zrušit rovnání",
            "dxDataGrid-editingSaveAllChanges": "Uložit změny",
            "dxDataGrid-editingCancelAllChanges": "Zrušit změny",
            "dxDataGrid-editingAddRow": "Přidat řádek",
            "dxDataGrid-summaryMin": "Min: {0}",
            "dxDataGrid-summaryMinOtherColumn": "Min {1} je {0}",
            "dxDataGrid-summaryMax": "Max: {0}",
            "dxDataGrid-summaryMaxOtherColumn": "Max {1} je {0}",
            "dxDataGrid-summaryAvg": "Prům.: {0}",
            "dxDataGrid-summaryAvgOtherColumn": "Průměr ze {1} je {0}",
            "dxDataGrid-summarySum": "Suma: {0}",
            "dxDataGrid-summarySumOtherColumn": "Suma {1} je {0}",
            "dxDataGrid-summaryCount": "Počet: {0}",
            "dxDataGrid-columnFixingFix": "Uchytit",
            "dxDataGrid-columnFixingUnfix": "Uvolnit",
            "dxDataGrid-columnFixingLeftPosition": "Vlevo",
            "dxDataGrid-columnFixingRightPosition": "Vpravo",
            "dxDataGrid-exportTo": "Export",
            "dxDataGrid-exportToExcel": "Export do sešitu Excel",
            "dxDataGrid-excelFormat": "soubor Excel",
            "dxDataGrid-selectedRows": "Vybrané řádky",
            "dxDataGrid-exportSelectedRows": "Export vybraných řádků",
            "dxDataGrid-exportAll": "Exportovat všechny záznamy",
            "dxDataGrid-headerFilterEmptyValue": "(prázdné)",
            "dxDataGrid-headerFilterOK": "OK",
            "dxDataGrid-headerFilterCancel": "Zrušit",
            "dxDataGrid-ariaColumn": "Sloupec",
            "dxDataGrid-ariaValue": "Hodnota",
            "dxDataGrid-ariaFilterCell": "Filtrovat buňku",
            "dxDataGrid-ariaCollapse": "Sbalit",
            "dxDataGrid-ariaExpand": "Rozbalit",
            "dxDataGrid-ariaDataGrid": "Datová mřížka",
            "dxDataGrid-ariaSearchInGrid": "Hledat v datové mřížce",
            "dxDataGrid-ariaSelectAll": "Vybrat vše",
            "dxDataGrid-ariaSelectRow": "Vybrat řádek",
            "dxTreeList-ariaTreeList": "Tree list",
            "dxTreeList-editingAddRowToNode": "Přidat",
            "dxPager-infoText": "Strana {0} ze {1} ({2} položek)",
            "dxPager-pagesCountText": "ze",
            "dxPivotGrid-grandTotal": "Celkem",
            "dxPivotGrid-total": "{0} Celkem",
            "dxPivotGrid-fieldChooserTitle": "Výběr pole",
            "dxPivotGrid-showFieldChooser": "Zobrazit výběr pole",
            "dxPivotGrid-expandAll": "Rozbalit vše",
            "dxPivotGrid-collapseAll": "Sbalit vše",
            "dxPivotGrid-sortColumnBySummary": 'Srovnat "{0}" podle tohoto sloupce',
            "dxPivotGrid-sortRowBySummary": 'Srovnat "{0}" podle tohoto řádku',
            "dxPivotGrid-removeAllSorting": "Odstranit veškeré třídění",
            "dxPivotGrid-dataNotAvailable": "nedostupné",
            "dxPivotGrid-rowFields": "Pole řádků",
            "dxPivotGrid-columnFields": "Pole sloupců",
            "dxPivotGrid-dataFields": "Pole dat",
            "dxPivotGrid-filterFields": "Filtrovat pole",
            "dxPivotGrid-allFields": "Všechna pole",
            "dxPivotGrid-columnFieldArea": "Zde vložte pole sloupců",
            "dxPivotGrid-dataFieldArea": "Zde vložte pole dat",
            "dxPivotGrid-rowFieldArea": "Zde vložte pole řádků",
            "dxPivotGrid-filterFieldArea": "Zde vložte filtr pole",
            "dxScheduler-editorLabelTitle": "Předmět",
            "dxScheduler-editorLabelStartDate": "Počáteční datum",
            "dxScheduler-editorLabelEndDate": "Koncové datum",
            "dxScheduler-editorLabelDescription": "Popis",
            "dxScheduler-editorLabelRecurrence": "Opakovat",
            "dxScheduler-openAppointment": "Otevřít schůzku",
            "dxScheduler-recurrenceNever": "Nikdy",
            "dxScheduler-recurrenceDaily": "Denně",
            "dxScheduler-recurrenceWeekly": "Týdně",
            "dxScheduler-recurrenceMonthly": "Měsíčně",
            "dxScheduler-recurrenceYearly": "Ročně",
            "dxScheduler-recurrenceEvery": "Každý",
            "dxScheduler-recurrenceEnd": "Konec opakování",
            "dxScheduler-recurrenceAfter": "Po",
            "dxScheduler-recurrenceOn": "Zap",
            "dxScheduler-recurrenceRepeatDaily": "dní",
            "dxScheduler-recurrenceRepeatWeekly": "týdnů",
            "dxScheduler-recurrenceRepeatMonthly": "měsíců",
            "dxScheduler-recurrenceRepeatYearly": "roků",
            "dxScheduler-switcherDay": "Den",
            "dxScheduler-switcherWeek": "Týden",
            "dxScheduler-switcherWorkWeek": "Pracovní týden",
            "dxScheduler-switcherMonth": "Měsíc",
            "dxScheduler-switcherAgenda": "Agenda",
            "dxScheduler-switcherTimelineDay": "Časová osa den",
            "dxScheduler-switcherTimelineWeek": "Časová osa týden",
            "dxScheduler-switcherTimelineWorkWeek": "Časová osa pracovní týden",
            "dxScheduler-switcherTimelineMonth": "Časová osa měsíc",
            "dxScheduler-recurrenceRepeatOnDate": "na den",
            "dxScheduler-recurrenceRepeatCount": "výskytů",
            "dxScheduler-allDay": "Celý den",
            "dxScheduler-confirmRecurrenceEditMessage": "Chcete upravit pouze tuto schůzku nebo celou sérii?",
            "dxScheduler-confirmRecurrenceDeleteMessage": "Chcete smazat pouze tuto schůzku nebo celou sérii?",
            "dxScheduler-confirmRecurrenceEditSeries": "Upravit sérii",
            "dxScheduler-confirmRecurrenceDeleteSeries": "Smazat sérii",
            "dxScheduler-confirmRecurrenceEditOccurrence": "Upravit schůzku",
            "dxScheduler-confirmRecurrenceDeleteOccurrence": "Smazat schůzku",
            "dxScheduler-noTimezoneTitle": "Bez časové zóny",
            "dxScheduler-moreAppointments": "{0} navíc",
            "dxCalendar-todayButtonText": "Dnes",
            "dxCalendar-ariaWidgetName": "Kalendář",
            "dxColorView-ariaRed": "Červená",
            "dxColorView-ariaGreen": "Zelená",
            "dxColorView-ariaBlue": "Modrá",
            "dxColorView-ariaAlpha": "Průhledná",
            "dxColorView-ariaHex": "Kód barvy",
            "dxTagBox-selected": "{0} vybráno",
            "dxTagBox-allSelected": "Vše vybráno ({0})",
            "dxTagBox-moreSelected": "{0} navíc",
            "vizExport-printingButtonText": "Tisk",
            "vizExport-titleMenuText": "Export/import",
            "vizExport-exportButtonText": "{0} souborů",
            "dxFilterBuilder-and": "A",
            "dxFilterBuilder-or": "NEBO",
            "dxFilterBuilder-notAnd": "NAND",
            "dxFilterBuilder-notOr": "NOR",
            "dxFilterBuilder-addCondition": "Přidat podmínku",
            "dxFilterBuilder-addGroup": "Přidat skupinu",
            "dxFilterBuilder-enterValueText": "<vložte hodnotu>",
            "dxFilterBuilder-filterOperationEquals": "Rovná se",
            "dxFilterBuilder-filterOperationNotEquals": "Nerovná se",
            "dxFilterBuilder-filterOperationLess": "Menší než",
            "dxFilterBuilder-filterOperationLessOrEquals": "Menší nebo rovno než",
            "dxFilterBuilder-filterOperationGreater": "Větší než",
            "dxFilterBuilder-filterOperationGreaterOrEquals": "Větší nebo rovno než",
            "dxFilterBuilder-filterOperationStartsWith": "Začíná na",
            "dxFilterBuilder-filterOperationContains": "Obsahuje",
            "dxFilterBuilder-filterOperationNotContains": "Neobsahuje",
            "dxFilterBuilder-filterOperationEndsWith": "Končí na",
            "dxFilterBuilder-filterOperationIsBlank": "Je prázdné",
            "dxFilterBuilder-filterOperationIsNotBlank": "Není prázdné"
        }
    })
});
