<template>
  <v-card class="grid-card">
    <div class="v-card__title">
      <h2>
        {{ title }}
      </h2>
    </div>

    <v-card-title class="search-wrap" v-if="showSearch || !hideExport || showInsert">
      <v-text-field v-if="showSearch" v-model="search" append-icon="search" :label="$t('common.search')" v-on:keyup.enter="filterGrid" single-line hide-details>
      </v-text-field>

      <v-spacer></v-spacer>

      <v-btn v-if="!hideExport" class="orange" @click="exportExcel">
        <v-icon color="white--text">
          arrow_downward
        </v-icon>
        {{ $t('common.export') }}
      </v-btn>

      <v-btn v-if="showInsert" class="orange" slot="activator" @click="addNewItem">
        <v-icon color="white--text">
          add
        </v-icon>
        {{ $t('common.add') }}
      </v-btn>
    </v-card-title>
    <div class="v-card__text">
      <v-data-table id="section-to-print" :headers="headers" :items="items" :loading="loading" :pagination.sync="pagination" :total-items="totalCount" :hide-actions="hideActions" :rows-per-page-text="$t('common.rowsPerPage')" :no-data-text="$t('common.noDataAvailable')" :rows-per-page-items="[10, 20, { 'text': $t('common.all'), 'value': -1 }]">
        <v-progress-linear slot="progress" color="teal" indeterminate>
        </v-progress-linear>

        <template slot="items" slot-scope="props">
          <td v-for="(header, i) in headers" :key="i">
            <template v-if="!header.isAction && header.isCheck">
              <div v-if="props.item[header.value]">
                <v-icon class="green--text">check</v-icon>
              </div>

              <div v-else>
                <v-icon class="red--text">close</v-icon>
              </div>
            </template>

            <template v-else-if="!header.isAction && header.isDateTime">
              {{ props.item[header.value] | moment("DD.MM.YYYY HH:mm") }}
            </template>

             <template v-else-if="!header.isAction && header.overtimeValue">
               <span v-if="props.item[header.isOvertime]" class="overtimeMinutes"> {{ props.item[header.value] }} {{ $t('common.minute') }} </span>
               <span v-else class="belowtimeMinutes"> {{ props.item[header.value] }} {{ $t('common.minute') }} </span>
            </template>

            <template v-else-if="!header.isAction && header.isDate">
              {{ props.item[header.value] | moment("DD.MM.YYYY") }}
            </template>

            <template v-else-if="!header.isAction">
              {{ props.item[header.value] }}
            </template>

            <template v-else>
              <v-btn v-if="showCalendar" icon class="mx-0" @click="calendarItem(props.item)">
                <v-icon color="#232222">
                  date_range
                </v-icon>
              </v-btn>

              <v-tooltip top>
                <v-btn v-if="showDetail" slot="activator" icon class="mx-0" @click="detailItem(props.item)">
                  <v-icon>
                    visibility
                  </v-icon>
                </v-btn>
                <span>{{ $t('common.viewRecord') }}</span>
              </v-tooltip>

              <v-tooltip top>
                <v-btn v-if="showEdit" slot="activator" icon class="mx-0" @click="editItem(props.item)">
                  <v-icon>
                    edit
                  </v-icon>
                </v-btn>
                <span>{{ $t('common.editRecord') }}</span>
              </v-tooltip>

              <v-tooltip top>
                <v-btn v-if="showDelete" slot="activator" icon class="mx-0" @click="deleteItem(props.item)">
                  <v-icon>
                    delete
                  </v-icon>
                </v-btn>
                <span>{{ $t('common.deleteRecord') }}</span>
              </v-tooltip>

              <v-btn v-if="showResetPassword" icon class="mx-0" @click="resetPassword(props.item)">
                <v-icon>
                  lock_open
                </v-icon>
              </v-btn>
            </template>
          </td>
        </template>
      </v-data-table>
    </div>
  </v-card>
</template>

<script>
export default {
    props: {
        items: {
            type: Array,
            required: false,
            default() {
                return [];
            }
        },

        loading: {
            type: Boolean,
            required: false
        },

        hideActions: {
            type: Boolean,
            required: false
        },

        headers: {
            type: Array,
            required: false
        },

        title: {
            type: String,
            required: false
        },

        showCalendar: {
            type: Boolean,
            required: false
        },

        showDetail: {
            type: Boolean,
            required: false
        },

        showEdit: {
            type: Boolean,
            required: false
        },

        showDelete: {
            type: Boolean,
            required: false
        },

        showSearch: {
            type: Boolean,
            required: false
        },

        showInsert: {
            type: Boolean,
            required: false
        },

        hideExport: {
            type: Boolean,
            required: false
        },

        showResetPassword: {
            type: Boolean,
            required: false
        },

        customParameters: {
            type: Object,
            required: false
        },

        methodName: {
            type: Function,
            required: true
        },

        totalCount: {
            type: Number,
            required: false
        }
    },
    data() {
        return {
            search: '',
            dataRows: [],
            pagination: {}
        };
    },

    watch: {
        pagination: {
            handler() {
                const vm = this;

                vm.executeGridOperations();
            }
        }
    },

    methods: {
        addNewItem() {
            const vm = this;

            //When the add new button is clicked, the event is sent to the grid component
            vm.$emit('newaction');
        },

        exportExcel() {
            const vm = this;

            //When the add new button is clicked, the event is sent to the grid component
            vm.$emit('exportToExcel');
        },

        filterGrid() {
            const vm = this;

            vm.executeGridOperations(true);
        },

        calendarItem(item) {
            const vm = this;

            //When the date range button is clicked, the event is sent to the grid component
            vm.$emit('calendar', item);
        },

        detailItem(item) {
            const vm = this;

            //When the detail button is clicked, the event is sent to the grid component
            vm.$emit('detail', item);
        },

        editItem(item) {
            const vm = this;

            //When the edit button is clicked, the event is sent to the grid component
            vm.$emit('edit', item, vm.items.indexOf(item));
        },

        deleteItem(item) {
            const vm = this;

            //When the delete button is clicked, the event is sent to the grid component
            vm.$emit('deleteitem', item);
        },

        resetPassword(item) {
            const vm = this;

            //When the reset password button is clicked, the event is sent to the grid component
            vm.$emit('resetpassword', item);
        },

        executeGridOperations(resetPageCount) {
            const vm = this;

            if (resetPageCount) {
                vm.pagination.page = 1;
            }

            var params = {};

            if (vm.customParameters) {
                params = vm.customParameters;
            }

            const { sortBy, descending, page, rowsPerPage } = vm.pagination;

            (params.currentPage = page),
                (params.pageSize = rowsPerPage),
                (params.search = vm.search),
                (params.sortBy = vm.getSortByField(sortBy)),
                (params.descending = descending);

            vm.$store.dispatch(vm.methodName(), params);
        },

        getSortByField(sortBy) {
            const vm = this;

            vm.headers.forEach(element => {
                if (element.value == sortBy && element.sortBy) {
                    sortBy = element.sortBy;
                }
            });

            return sortBy;
        }
    }
};
</script>

<style>
@media print {
    body * {
        visibility: hidden;
    }
    #section-to-print,
    #section-to-print * {
        visibility: visible;
    }
    .v-content {
        position: relative;
    }
    #section-to-print {
        position: absolute;
        left: 0;
        top: 0;
    }
    .navigation.v-navigation-drawer {
        display: none !important;
    }
    main.v-content {
        padding: 0 !important;
    }
}
.v-table__overflow .v-btn__content .v-icon {
    color: #000;
}
.v-card__text + .btn-wrap {
    margin-top: 40px;
}
.grid-card.card {
    box-shadow: inherit;
    height: 100vh !important;
}
.datatable__actions {
    background-color: #f8f8f8 !important;
    padding-top: 10px;
}
.table__overflow {
    margin-top: 36px;
}
table.v-table thead tr {
    height: 70px;
}
table.v-table thead th {
    font-weight: bold;
    font-size: initial;
}
table.v-table thead td {
    font-weight: 400;
    font-size: 75%;
}
table.v-table tbody td:last-child {
    text-align: right;
}
tr:nth-child(even) {
    background-color: #f2f2f2;
}

.headline-wrap .btn--active .btn__content:before,
.headline-wrap .btn:focus .btn__content:before,
.headline-wrap .btn:hover .btn__content:before {
    background-color: transparent;
}
.headline-wrap .backBtn i {
    color: #000 !important;
}
.headline-wrap .v-btn__content {
    will-change: box-shadow;
    -webkit-box-shadow: 0 3px 1px -2px rgba(0, 0, 0, 0.2),
        0 2px 2px 0 rgba(0, 0, 0, 0.14), 0 1px 5px 0 rgba(0, 0, 0, 0.12);
    box-shadow: 0 3px 1px -2px rgba(0, 0, 0, 0.2),
        0 2px 2px 0 rgba(0, 0, 0, 0.14), 0 1px 5px 0 rgba(0, 0, 0, 0.12);
}
.label {
    font-size: 12px;
    line-height: 1.33;
    text-align: left;
    color: #464646;
    margin-bottom: 5px;
}
.value {
    font-size: 16px;
    line-height: 1.5;
    text-align: left;
    color: #000;
}

a {
    color: #009688;
}

.input-group-checkbox {
    display: flex;
    align-items: center;
}
.readonly-wrap {
    flex-direction: column;
}
.input-group.readonly-wrap {
    margin-bottom: 30px;
    background-color: #f7f7f7;
    padding: 10px;
    min-height: 64px;
}

.input-group__selections > div {
    display: inline-block !important;
    width: 100%;
    white-space: nowrap;
    overflow: hidden !important;
    text-overflow: ellipsis;
}
table.v-table tbody td:first-child,
table.v-table tbody td:not(:first-child),
table.v-table tbody th:first-child,
table.v-table tbody th:not(:first-child),
table.v-table thead td:first-child,
table.v-table thead td:not(:first-child),
table.v-table thead th:first-child,
table.v-table thead th:not(:first-child) {
    padding: 0px 15px;
    white-space: nowrap;
}
.navigation i,
.navigation .v-list__tile__title {
    color: #fff !important;
}
.application .theme--light.v-list .v-list__group__header:hover,
.application .theme--light.v-list .v-list__tile--highlighted,
.application .theme--light.v-list .v-list__tile--link:hover,
.theme--light .v-list .v-list__group__header:hover,
.theme--light .v-list .v-list__tile--highlighted,
.theme--light .v-list .v-list__tile--link:hover {
    background: rgba(0, 0, 0, 0.1);
}
.v-navigation-drawer .v-list {
    background: #333;
}
.v-list__group.v-list__group--active {
    background: #262626;
}
.v-toolbar__content .v-input {
    position: relative;
}
.v-toolbar__content .v-input .v-input__prepend-outer {
    position: absolute;
    right: 10px;
    top: 0;
}
.v-toolbar__content .v-toolbar__title button {
    margin-left: -11px;
}
.v-toolbar__content .v-toolbar__title {
    overflow: visible;
}
.v-select.v-select--chips .v-select__selections {
    min-height: 32px;
}

input[type='date']::-webkit-inner-spin-button {
    display: none;
    -webkit-appearance: none;
}
.v-chip__content {
    background-color: #ee8b41;
    color: #fff !important;
}
.overtimeMinutes {
    color: red !important;
}
.belowtimeMinutes {
    color: green !important;
}
</style>
