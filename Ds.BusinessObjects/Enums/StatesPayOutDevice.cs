using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Enums
{
    public enum StatesPayOutDevice
    {
        ConexionExitosa,
        ErrorConexion,
        Nothing,
        Reset,
        ValidadorReady,
        ErrorValidador,
        EnableValidator,
        DisableValidator,
        DisableValidatorError,
        SSP_POLL_SLAVE_RESET,
        SSP_POLL_DISABLED,
        SSP_POLL_READ_NOTE,
        SSP_POLL_CREDIT_NOTE,
        SSP_POLL_NOTE_REJECTING,
        SSP_POLL_NOTE_REJECTED,
        SSP_POLL_NOTE_STACKING,
        SSP_POLL_FLOATING,
        SSP_POLL_NOTE_STACKED,
        SSP_POLL_FLOATED,
        SSP_POLL_NOTE_STORED_IN_PAYOUT,
        SSP_POLL_SAFE_NOTE_JAM,
        SSP_POLL_UNSAFE_NOTE_JAM,
        SSP_POLL_ERROR_DURING_PAYOUT,
        SSP_POLL_FRAUD_ATTEMPT,
        SSP_POLL_STACKER_FULL,
        SSP_POLL_NOTE_CLEARED_FROM_FRONT,
        SSP_POLL_NOTE_CLEARED_TO_CASHBOX,
        SSP_POLL_NOTE_PAID_INTO_STORE_AT_POWER_UP,
        SSP_POLL_NOTE_PAID_INTO_STACKER_AT_POWER_UP,
        SSP_POLL_CASHBOX_REMOVED,
        SSP_POLL_CASHBOX_REPLACED,
        SSP_POLL_DISPENSING,
        SSP_POLL_DISPENSED,
        SSP_POLL_EMPTYING,
        SSP_POLL_EMPTIED,
        SSP_POLL_SMART_EMPTYING,
        SSP_POLL_SMART_EMPTIED,
        SSP_POLL_JAMMED,
        SSP_POLL_HALTED,
        SSP_POLL_INCOMPLETE_PAYOUT,
        SSP_POLL_INCOMPLETE_FLOAT,
        SSP_POLL_NOTE_TRANSFERED_TO_STACKER,
        SSP_POLL_NOTE_HELD_IN_BEZEL,
        SSP_POLL_TIME_OUT,
        DISPENSING,
        DISPENSED,

    }
    public enum TipoInsertEventoPayOut
    {
        Ninguno,
        Envio,
        Recepcion,
    }
}
