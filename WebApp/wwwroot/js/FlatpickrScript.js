
/**
* Reserved dates are passed from the controller using ViewBag and serialized into a JS array.
* These are the dates when the equipment is already booked (status = approved).
*/
const reservedDates = @Html.Raw(System.Text.Json.JsonSerializer.Serialize((List < string >)ViewBag.UnavailableDates));

// Helper: Checks if a specific date is reserved
function isReservedDate(date) {
    const iso = date.toISOString().split('T')[0];
    return reservedDates.includes(iso);
}

// Helper: Finds the first available date from today onwards that is NOT reserved
function getFirstAvailableDate() {
    const today = new Date();
    for (let i = 0; i < 365; i++) {
        const testDate = new Date(today);
        testDate.setDate(today.getDate() + i);
        const iso = testDate.toISOString().split('T')[0];
        if (!reservedDates.includes(iso)) {
            return iso;
        }
    }
    return null;
}

// Get the default starting date (first available day)
const firstAvailableDate = getFirstAvailableDate();

/**
 * Initialize the RETURN DATE picker
 * - Disables all reserved dates
 * - Highlights reserved days in green
 * - Min date will be set dynamically based on selected start date
 */
const returnPicker = flatpickr("#returnDate", {
    dateFormat: "Y-m-d",
    minDate: firstAvailableDate,
    disable: reservedDates,
    onDayCreate: function (dObj, dStr, fp, dayElem) {
        const date = dayElem.dateObj;
        if (isReservedDate(date)) {
            dayElem.classList.add("reserved-day"); // add green background
        }
    }
});

/**
 * Initialize the START DATE picker
 * - Disables all reserved dates
 * - Highlights reserved days in green
 * - Sets the default date to the first available date
 * - On change: updates the return date min & max boundaries to avoid overlap
 */
const startPicker = flatpickr("#startDate", {
    dateFormat: "Y-m-d",
    minDate: "today",
    disable: reservedDates,
    defaultDate: firstAvailableDate,
    onChange: function (selectedDates, dateStr) {
        if (dateStr) {
            const startDate = new Date(dateStr);
            let maxDate = null;

            // Find the next reserved date after the start date
            for (let i = 1; i < 30; i++) {
                const checkDate = new Date(startDate);
                checkDate.setDate(startDate.getDate() + i);
                const iso = checkDate.toISOString().split('T')[0];
                if (reservedDates.includes(iso)) {
                    maxDate = new Date(checkDate);
                    maxDate.setDate(maxDate.getDate() - 1); // set max return date = day before reserved
                    break;
                }
            }

            // Update the returnPicker boundaries
            returnPicker.set('minDate', dateStr);
            returnPicker.set('maxDate', maxDate);
        }
    },
    onDayCreate: function (dObj, dStr, fp, dayElem) {
        const date = dayElem.dateObj;
        if (isReservedDate(date)) {
            dayElem.classList.add("reserved-day"); // add green background
        }
    }
});

/**
 * Calculates total rental price based on selected dates
 * - Displays cost per day, total cost, and total days
 */
function calculateDaysAndTotal() {
    const start = new Date(document.getElementById('startDate').value);
    const end = new Date(document.getElementById('returnDate').value);
    const perDay = parseFloat('@equipment?.RentalPricePerDay' || 0);

    if (!isNaN(start.getTime()) && !isNaN(end.getTime()) && end >= start) {
        const days = Math.ceil((end - start) / (1000 * 60 * 60 * 24)) + 1;
        const total = perDay * days;
        document.getElementById('priceDetails').textContent =
            `${perDay.toFixed(2)} BD/Day → ${total.toFixed(2)} BD/${days} Days`;
    } else {
        document.getElementById('priceDetails').textContent =
            `${perDay.toFixed(2)} BD/Day → 0 BD/0 Days`;
    }
}

// Bind price calculation on both date inputs
document.getElementById('startDate').addEventListener('change', calculateDaysAndTotal);
document.getElementById('returnDate').addEventListener('change', calculateDaysAndTotal);
