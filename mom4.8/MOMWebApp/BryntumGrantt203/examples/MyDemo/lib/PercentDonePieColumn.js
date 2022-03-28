import PercentDoneColumn from '../../../lib/Gantt/column/PercentDoneColumn.js';
import ColumnStore from '../../../lib/Grid/data/ColumnStore.js';

/**
 * @module PercentDonePieColumn
 */

/**
 * A column drawing a pie chart of the `percentDone` value
 *
 * @extends Gantt/column/PercentDoneColumn
 * @classType percentdonepie
 */
export default class PercentDonePieColumn extends PercentDoneColumn {
    static get type() {
        return 'percentdonepie';
    }

    static get isGanttColumn() {
        return true;
    }

    static get defaults() {
        return {
            // Set your default instance config properties here
            htmlEncode : false,
            align      : 'center'
        };
    }

    //endregion

    renderer({ value }) {
        return value ? {
            className : 'b-pie',
            style     : {
                animationDelay : `-${value}s`
            }
        } : '';
    }
}

ColumnStore.registerColumnType(PercentDonePieColumn);
