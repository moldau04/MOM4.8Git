import ResourceAssignmentColumn from '../../../lib/Gantt/column/ResourceAssignmentColumn.js';
import ColumnStore from '../../../lib/Grid/data/ColumnStore.js';

/**
 * @module ResourceAvatarColumn
 */

const defaultImagePath = '../_shared/images/users/';

/**
 * Column showing avatars of the assigned resource
 *
 * @extends Grid/column/Column
 * @classType resourceavatar
 */
export default class ResourceAvatarColumn extends ResourceAssignmentColumn {
    static get type() {
        return 'resourceavatar';
    }

    static get isGanttColumn() {
        return true;
    }

    static get fields() {
        return [
            /**
             * The path for the resource images
             * @config {String}
             * @category Common
             */
            'imageFolderPath'
        ];
    }

    static get defaults() {
        return {
            repaintOnResize : true,
            htmlEncode      : false,
            cellCls         : 'b-resource-avatar-cell',
            imageFolderPath : defaultImagePath,
            autoSyncHtml    : true
        };
    }

    renderer({ value }) {
        const
            imgSize = 30,
            nbrVisible = Math.floor((this.width - 20) / (imgSize + 2)),
            lastIndex = nbrVisible - 1,
            overflowCount = value.length - nbrVisible;

        return '<div class="b-resource-avatar-container">' + Array.from(value).map((assignment, i) => {
            const { resource } = assignment;

            let markup = '';

            if (resource && i < nbrVisible) {
                const
                    isLastOverflowing = i === lastIndex && overflowCount > 0,
                    imgBtip = `${resource.name} ${assignment.units}%${isLastOverflowing ? ` (+${overflowCount} more resources)` : ''}`,
                    imgMarkup = `<img data-btip-scroll-action="realign" data-btip="${imgBtip}" class="b-resource-avatar" src="${this.imageFolderPath}${resource.name.toLowerCase() || 'none'}.jpg">`;

                if (isLastOverflowing) {
                    markup = `<div class="b-overflow-img">${imgMarkup}<span class="b-overflow-count">+${overflowCount}</span></div>`;
                }
                else {
                    markup = imgMarkup;
                }
            }

            return markup;
        }).join('') + '</div>';
    }
}

ColumnStore.registerColumnType(ResourceAvatarColumn);
