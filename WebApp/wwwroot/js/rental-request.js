//  Parse reserved dates from JSON embedded in the HTML <script> tag
// Example source: <script id="reservedDatesData" type="application/json">["2025-06-10", "2025-06-15"]</script>
const reservedDates = JSON.parse(document.getElementById("reservedDatesData").textContent.trim());

console.log("Hello");
/**
 *  Checks if a given date (JS Date object) is within the reserved dates
 * @param {Date} date
 * @returns {boolean} - true if date is in reserved list
 */
function isReservedDate(date) {
    const iso = date.toISOString().split('T')[0];
    return reservedDates.includes(iso);
}

//  Get DOM elements for start and return date inputs
const startInput = document.getElementById('startDate');
const returnInput = document.getElementById('returnDate');

//  Parse current values of start and return dates if pre-filled (e.g., during edit)
const startVal = startInput?.value?.split('T')[0] || null;
const returnVal = returnInput?.value?.split('T')[0] || null;
const isCreatePage = !startVal && !returnVal;  // true only if no dates are initially set (create mode)

// ----------------------------------------------------------------------------------
//  Initialize Flatpickr for START date
// ----------------------------------------------------------------------------------
const startPicker = flatpickr("#startDate", {
    dateFormat: "Y-m-d",
    enableTime: false,
    minDate: "today",
    defaultDate: isCreatePage ? null : startVal,
    disable: reservedDates,

    //  When user selects a new start date
    onChange: function (selectedDates, dateStr) {
        // Update return picker's minimum date to prevent selecting before start
        returnPicker.set("minDate", dateStr);
        returnPicker.redraw(); // Repaint calendar to reflect conflict days
        calculateDaysAndTotal(); // Refresh cost/duration
    },

    //  Add reserved day styling
    onDayCreate: function (_, __, ___, dayElem) {
        if (isReservedDate(dayElem.dateObj)) {
            dayElem.classList.add("reserved-day"); // Add green marker or any reserved style
        }
    }
});

// ----------------------------------------------------------------------------------
//  Initialize Flatpickr for RETURN date
// ----------------------------------------------------------------------------------
const returnPicker = flatpickr("#returnDate", {
    dateFormat: "Y-m-d",
    enableTime: false,
    minDate: startVal || "today",  // fallback if startVal is null
    defaultDate: isCreatePage ? null : returnVal,
    disable: reservedDates,

    //  Called every time return date value changes
    onValueUpdate: function (selectedDates, dateStr, instance) {
        const selectedStart = startPicker.selectedDates?.[0];
        const selectedReturn = selectedDates?.[0];

        //  Prevent selecting same day for start and return
        if (selectedStart && selectedReturn) {
            const returnIso = selectedReturn.toISOString().split('T')[0];
            const startIso = selectedStart.toISOString().split('T')[0];

            if (returnIso === startIso) {
                instance.clear();
                showFlash("Return date cannot be the same as start date.", "warning"); // Visual message
                return;
            }
        }

        calculateDaysAndTotal(); // Update totals if date is valid
    },

    //  Add reserved day styling and prevent selection of start date as return
    onDayCreate: function (_, __, ___, dayElem) {
        const iso = dayElem.dateObj.toISOString().split('T')[0];
        const selectedStart = startPicker.selectedDates?.[0];
        const selectedStartIso = selectedStart ? selectedStart.toISOString().split('T')[0] : null;

        if (isReservedDate(dayElem.dateObj)) {
            dayElem.classList.add("reserved-day");
        }

        //  Highlight same-day return as a conflict
        if (selectedStartIso && iso === selectedStartIso) {
            dayElem.classList.add("conflict-day");
            dayElem.setAttribute("title", "Return date cannot be the same as start date.");
        }
    }
});

// ----------------------------------------------------------------------------------
//  Calculates total rental days and cost
// ----------------------------------------------------------------------------------
function calculateDaysAndTotal() {
    const startRaw = startInput?.value?.split('T')[0];
    const endRaw = returnInput?.value?.split('T')[0];
    const perDay = parseFloat(document.getElementById('rentalPerDay')?.value || "0");

    const priceDetails = document.getElementById('priceDetails'); // Optional display span
    const numberOfDays = document.getElementById('numberOfDays'); // Input for total days
    const totalCost = document.getElementById('totalCost');       // Input for total cost

    //  If no dates are selected yet
    if (!startRaw || !endRaw) {
        if (priceDetails) priceDetails.textContent = `${perDay.toFixed(2)} BD/Day → 0 BD/0 Days`;
        if (numberOfDays) numberOfDays.value = '';
        if (totalCost) totalCost.value = '';
        return;
    }

    const start = new Date(startRaw + "T00:00:00");
    const end = new Date(endRaw + "T00:00:00");

    //  Invalid selection: return before start, or same day
    if (start.getTime() === end.getTime() || end < start) {
        if (priceDetails) priceDetails.textContent = `${perDay.toFixed(2)} BD/Day → 0 BD/0 Days`;
        if (numberOfDays) numberOfDays.value = '';
        if (totalCost) totalCost.value = '';
        return;
    }

    //  Valid: calculate days and total cost
    const days = Math.ceil((end - start) / (1000 * 60 * 60 * 24)) + 1;
    const total = perDay * days;

    if (priceDetails) priceDetails.textContent = `${perDay.toFixed(2)} BD/Day → ${total.toFixed(2)} BD/${days} Days`;
    if (numberOfDays) numberOfDays.value = days;
    if (totalCost) totalCost.value = total.toFixed(2);
}

// ----------------------------------------------------------------------------------
//  Trigger real-time recalculation on manual change
// ----------------------------------------------------------------------------------
startInput?.addEventListener('change', calculateDaysAndTotal);
returnInput?.addEventListener('change', calculateDaysAndTotal);

//  Calculate total automatically on page load
window.addEventListener('load', () => {
    setTimeout(calculateDaysAndTotal, 100);
});
